
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckSims;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.ModelResponses.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.eWalletTransaction;
using _24hplusdotnetcore.Repositories.MC;
using _24hplusdotnetcore.Services.CheckSims;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Settings;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ICheckSimService
    {
        Task<SendOtpResponse> SendOtp(SendOtpRequest request);
        Task<SendOtpResponse> ResendOtp(string id);
        Task<Scoring3PResponse> SendScoring3P(Scoring3PRequest request);
        Task<PagingResponse<GetCheckSimResponse>> GetAsync(GetCheckSimRequest request);
        Task<dynamic> CheckSimAsync(PtfScopingCheckSimRequest request);
        Task<dynamic> SendOtpAsync(PtfScopingSendOtpRequest request);
        Task<dynamic> VerifyOtpAsync(PtfScopingVerifyOtpRequest request);
    }
    public class CheckSimService : ICheckSimService, IScopedLifetime
    {
        private readonly ILogger<CheckSimService> _logger;
        private readonly IRestMCService _restMCService;
        private readonly IMapper _mapper;
        private readonly MCConfig _mCConfig;
        private readonly ConfigServices _configServices;
        private readonly IUserLoginService _userLoginService;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITrustingSocialRepository _trustingSocialRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICheckSimRepository _checkSimRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMongoRepository<POS> _posRepository;
        private readonly IPtfScoringTrustingSocialRestService _ptfScoringTrustingSocialRestService;
        private readonly IPtfScoringVnptRestService _ptfScoringVnptRestService;
        private readonly PtfScoringConfig _ptfScoringConfig;
        private readonly IUserServices _userService;

        public CheckSimService(
            ILogger<CheckSimService> logger,
            IRestMCService restMCService,
            IMapper mapper,
            IOptions<MCConfig> mCConfigOptions,
            ConfigServices configServices,
            IUserLoginService userLoginService,
            INotificationRepository notificationRepository,
            ITrustingSocialRepository trustingSocialRepository,
            ICheckSimRepository checkSimRepository,
            ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            IMongoRepository<POS> posRepository,
            IPtfScoringTrustingSocialRestService ptfScoringTrustingSocialRestService,
            IUserServices userService,
            IPtfScoringVnptRestService ptfScoringVnptRestService,
            IOptions<PtfScoringConfig> ptfScoringConfigOptions
            )
        {
            _logger = logger;
            _restMCService = restMCService;
            _mapper = mapper;
            _mCConfig = mCConfigOptions.Value;
            _configServices = configServices;
            _userLoginService = userLoginService;
            _notificationRepository = notificationRepository;
            _trustingSocialRepository = trustingSocialRepository;
            _checkSimRepository = checkSimRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _posRepository = posRepository;
            _userService = userService;
            _ptfScoringTrustingSocialRestService = ptfScoringTrustingSocialRestService;
            _ptfScoringVnptRestService = ptfScoringVnptRestService;
            _ptfScoringConfig = ptfScoringConfigOptions.Value;
        }

        public async Task<string> GetMCTokenAsync()
        {
            var config = _configServices.FindOneByKey(ConfigKey.MC_AUTHORIZATION);
            return await Task.FromResult(config?.Value);
        }

        public async Task<SendOtpResponse> SendOtp(SendOtpRequest request)
        {

            // check transaction
            var trans = await _transactionRepository.FindByIdAsync(request.TransactionId);
            if (trans == null)
            {
                throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "giao dịch"));
            }
            else
            {
                if (trans.Status != TransactionStatus.SUCCEEDED)
                {
                    throw new ArgumentException(TransactionErrorMessage.WRONG_STATUS);
                }
                if (trans.Creator != _userLoginService.GetUserId())
                {
                    throw new ArgumentException(TransactionErrorMessage.NOT_OWNER);
                }
            }

            var sendOtp = _mapper.Map<CheckSim>(request);

            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            sendOtp.SaleInfo = saleInfo;
            sendOtp.TeamLeadInfo = teamLeadInfo;
            sendOtp.AsmInfo = asmInfo;
            sendOtp.PosInfo = posInfo;
            sendOtp.SaleChanelInfo = saleChanelInfo;

            try
            {
                var sendResult = new SendOtpResponse();

                SendOtpRequestValidation validator = new SendOtpRequestValidation();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    throw new ArgumentException(string.Join(", ", result.Errors.Select(x => x.ErrorMessage)));
                }

                // Create request
                var tranSim = _mapper.Map<CheckSimTransaction>(trans);
                sendOtp.Creator = _userLoginService.GetUserId();
                sendOtp.TransactionId = trans.Id;
                sendOtp.Transaction = tranSim;
                sendOtp.Project = CheckSimProject.MC;
                sendOtp.Action = CheckSimAction.SendOTP;
                sendOtp.AbsolutePath = $"{_mCConfig.Host}{HelperExtension.GetHttpMethodPath(typeof(IRestMCService), nameof(IRestMCService.SendOtp))}";


                await _checkSimRepository.InsertOneAsync(sendOtp);

                JObject response = null;
                SendOtpRestRequest sentRequest = null;
                if (_mCConfig.IsTestMode)
                {
                    sendResult = new SendOtpResponse()
                    {
                        ReturnCode = "success",
                        ReturnMes = "Gửi OTP thành công, Mã OTP là: 123456"
                    };
                }
                else
                {
                    sentRequest = new SendOtpRestRequest
                    {
                        RequestedMsisdn = request.RequestedMsisdn,
                        TypeScore = request.TypeScore,
                    };
                    string token = await GetMCTokenAsync();
                    response = await _restMCService.SendOtpV2(sentRequest, string.Format("{0} {1}", "Bearer", token));
                    sendResult = JsonConvert.DeserializeObject<SendOtpResponse>(response.ToString());

                }
                if (sendResult.ReturnCode == "success")
                {
                    var histories = trans.BillStepHistories?.ToList() ?? new List<BillStepHistories>();
                    histories.Add(new BillStepHistories()
                    {
                        Status = BillStatus.SENT_OTP
                    });
                    // Update Transactioin
                    trans.Status = TransactionStatus.PAYED;
                    var update = Builders<TransactionModel>.Update
                        .Set(x => x.Status, trans.Status)
                        .Set(x => x.BillType, BillType.MC_CHECK_SIM)
                        .Set(x => x.BillId, sendOtp.Id)
                        .Set(x => x.BillStatus, BillStatus.SENT_OTP)
                        .Set(x => x.BillPhoneNumber, request.RequestedMsisdn)
                        .Set(x => x.BillIdCard, request.IdCard)
                        .Set(x => x.BillStepHistories, histories)
                        .Set(x => x.ModifiedDate, DateTime.Now);
                    await _transactionRepository.UpdateOneAsync(x => x.Id == trans.Id, update);
                }

                // Update check sim
                var updateCheckSim = Builders<CheckSim>.Update
                    .Set(x => x.Payload, JsonConvert.SerializeObject(sentRequest))
                    .Set(x => x.Response, response?.ToString())
                    .Set(x => x.Transaction, tranSim)
                    .Set(x => x.ModifiedDate, DateTime.Now);
                await _checkSimRepository.UpdateOneAsync(x => x.Id == sendOtp.Id, updateCheckSim);

                return sendResult;
            }
            catch (ApiException ex)
            {
                var updateCheckSim = Builders<CheckSim>.Update
                    .Set(x => x.Message, ex.Message)
                    .Set(x => x.Response, ex.Content);
                await _checkSimRepository.UpdateOneAsync(x => x.Id == sendOtp.Id, updateCheckSim);

                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<SendOtpResponse> ResendOtp(string id)
        {

            // check transaction
            var trans = await _transactionRepository.FindByIdAsync(id);
            if (trans == null)
            {
                throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "giao dịch"));
            }
            else
            {
                if (trans.Creator != _userLoginService.GetUserId())
                {
                    throw new ArgumentException(TransactionErrorMessage.NOT_OWNER);
                }
            }
            SendOtpRequest request = new SendOtpRequest()
            {
                RequestedMsisdn = trans.BillPhoneNumber,
                TypeScore = trans.BillIdCard,
                TransactionId = id,
            };
            var sendOtp = _mapper.Map<CheckSim>(request);

            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            sendOtp.SaleInfo = saleInfo;
            sendOtp.TeamLeadInfo = teamLeadInfo;
            sendOtp.AsmInfo = asmInfo;
            sendOtp.PosInfo = posInfo;
            sendOtp.SaleChanelInfo = saleChanelInfo;

            try
            {
                var sendResult = new SendOtpResponse();

                SendOtpRequestValidation validator = new SendOtpRequestValidation();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    throw new ArgumentException(string.Join(", ", result.Errors.Select(x => x.ErrorMessage)));
                }

                // Create request
                var tranSim = _mapper.Map<CheckSimTransaction>(trans);
                sendOtp.Creator = _userLoginService.GetUserId();
                sendOtp.TransactionId = trans.Id;
                sendOtp.Transaction = tranSim;
                sendOtp.Project = CheckSimProject.MC;
                sendOtp.Action = CheckSimAction.ResendOTP;
                sendOtp.AbsolutePath = $"{_mCConfig.Host}{HelperExtension.GetHttpMethodPath(typeof(IRestMCService), nameof(IRestMCService.SendOtp))}";


                await _checkSimRepository.InsertOneAsync(sendOtp);

                JObject response = null;
                SendOtpRestRequest sentRequest = null;
                if (_mCConfig.IsTestMode)
                {
                    sendResult = new SendOtpResponse()
                    {
                        ReturnCode = "success",
                        ReturnMes = "Gửi OTP thành công, Mã OTP là: 123456"
                    };
                }
                else
                {
                    sentRequest = new SendOtpRestRequest
                    {
                        RequestedMsisdn = request.RequestedMsisdn,
                        TypeScore = request.TypeScore,
                    };
                    string token = await GetMCTokenAsync();
                    response = await _restMCService.SendOtpV2(sentRequest, string.Format("{0} {1}", "Bearer", token));
                    sendResult = JsonConvert.DeserializeObject<SendOtpResponse>(response.ToString());
                }

                if (sendResult.ReturnCode == "success")
                {
                    var histories = trans.BillStepHistories?.ToList() ?? new List<BillStepHistories>();
                    histories.Add(new BillStepHistories()
                    {
                        Status = BillStatus.SENT_OTP
                    });
                    // Update Transactioin
                    var updateTrans = Builders<TransactionModel>.Update
                        .Set(x => x.BillStatus, BillStatus.RESENT_OTP)
                        .Set(x => x.BillStepHistories, histories)
                        .Set(x => x.ModifiedDate, DateTime.Now);
                    await _transactionRepository.UpdateOneAsync(x => x.Id == trans.Id, updateTrans);
                }
                // Update check sim
                var updateCheckSim = Builders<CheckSim>.Update
                    .Set(x => x.Payload, JsonConvert.SerializeObject(sentRequest))
                    .Set(x => x.Response, response?.ToString())
                    .Set(x => x.Transaction, tranSim)
                    .Set(x => x.ModifiedDate, DateTime.Now);
                await _checkSimRepository.UpdateOneAsync(x => x.Id == sendOtp.Id, updateCheckSim);

                return sendResult;
            }
            catch (ApiException ex)
            {
                var updateCheckSim = Builders<CheckSim>.Update
                     .Set(x => x.Message, ex.Message)
                     .Set(x => x.Response, ex.Content);
                await _checkSimRepository.UpdateOneAsync(x => x.Id == sendOtp.Id, updateCheckSim);

                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Scoring3PResponse> SendScoring3P(Scoring3PRequest request)
        {
            try
            {
                var responseObj = new Scoring3PResponse();
                Scoring3PRequestValidation validator = new Scoring3PRequestValidation();
                ValidationResult result = validator.Validate(request);
                if (!result.IsValid)
                {
                    throw new ArgumentException(string.Join(", ", result.Errors.Select(x => x.ErrorMessage)));
                }
                JObject response = null;
                Scoring3PRestRequest sentRequest = null;
                if (_mCConfig.IsTestMode)
                {
                    responseObj = new Scoring3PResponse()
                    {
                        ReturnCode = "success",
                        ReturnMes = "Chấm điểm thành công! Số điểm là: 100"
                    };
                }
                else
                {
                    sentRequest = new Scoring3PRestRequest
                    {
                        NationalId = request.NationalId,
                        PrimaryPhone = request.PrimaryPhone,
                        TypeScore = request.TypeScore,
                        UserName = _mCConfig.Username,
                        VerificationCode = request.VerificationCode
                    };

                    string token = await GetMCTokenAsync();
                    response = await _restMCService.SendScoring3PV2(sentRequest, string.Format("{0} {1}", "Bearer", token));

                    responseObj = JsonConvert.DeserializeObject<Scoring3PResponse>(response.ToString());
                    responseObj.ReturnMes = MCTrustingSocialMapping.MESSAGE.TryGetValue(responseObj.ReturnMes, out string message) ? message : responseObj.ReturnMes;

                }

                // Save result of score
                var scoring = _mapper.Map<CheckSim>(responseObj);
                scoring.Creator = _userLoginService.GetUserId();
                scoring.Project = CheckSimProject.MC;
                scoring.Action = CheckSimAction.VerifyOTP;
                scoring.AbsolutePath = $"{_mCConfig.Host}{HelperExtension.GetHttpMethodPath(typeof(IRestMCService), nameof(IRestMCService.SendScoring3P))}";

                var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
                scoring.SaleInfo = saleInfo;
                scoring.TeamLeadInfo = teamLeadInfo;
                scoring.AsmInfo = asmInfo;
                scoring.PosInfo = posInfo;
                scoring.SaleChanelInfo = saleChanelInfo;

                await _checkSimRepository.InsertOneAsync(scoring);

                if (responseObj.ReturnCode == "success")
                {
                    // Update check sim
                    var userId = _userLoginService.GetUserId();
                    var curCheckSim = (await _checkSimRepository.FilterByAsync(x => x.PhoneNumber == request.PrimaryPhone
                        && x.Creator == userId
                        && x.TypeScore == request.TypeScore)).ToList();
                    if (curCheckSim != null && curCheckSim.Count > 0)
                    {
                        var curCheckSimId = curCheckSim.Last().Id;

                        var updateCheckSim = Builders<CheckSim>.Update
                            .Set(x => x.Payload, JsonConvert.SerializeObject(sentRequest))
                            .Set(x => x.Response, response?.ToString())
                            .Set(x => x.ModifiedDate, DateTime.Now);
                        await _checkSimRepository.UpdateOneAsync(x => x.Id == curCheckSimId, updateCheckSim);
                    }

                    // Update Trans
                    var trans = (await _transactionRepository.FilterByAsync(x => x.BillPhoneNumber == request.PrimaryPhone
                                                                            && x.BillStatus != BillStatus.INIT)).ToList();
                    if (trans != null && trans.Count > 0)
                    {
                        var tran = trans.Last();
                        var histories = tran.BillStepHistories?.ToList() ?? new List<BillStepHistories>();
                        histories.Add(new BillStepHistories()
                        {
                            Status = BillStatus.VERIFIED_OTP
                        });
                        var updateTrans = Builders<TransactionModel>.Update
                            .Set(x => x.BillStatus, BillStatus.VERIFIED_OTP)
                            .Set(x => x.BillStepHistories, histories)
                            .Set(x => x.ModifiedDate, DateTime.Now);
                        await _transactionRepository.UpdateOneAsync(x => x.Id == tran.Id, updateTrans);
                    }
                }

                return responseObj;
            }
            catch (ApiException ex)
            {
                var scoring = new MCTrustingSocial
                {
                    PayLoad = JsonConvert.SerializeObject(request),
                    Response = ex.Content,
                    Creator = _userLoginService.GetUserId()
                };

                await _trustingSocialRepository.Create(scoring);

                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetCheckSimResponse>> GetAsync(GetCheckSimRequest request)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.CheckSim_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.CheckSim_ViewAll,
                    PermissionCost.PosLeadPermission.CheckSim_ViewAll,
                    PermissionCost.AsmPermission.CheckSim_ViewAll,
                    PermissionCost.TeamLeaderPermission.CheckSim_ViewAll);

                var checkSims = await _checkSimRepository.GetAsync(filterByCreatorIds,request);
                var total = await _checkSimRepository.CountAsync(filterByCreatorIds, request);

                return new PagingResponse<GetCheckSimResponse>
                {
                    TotalRecord = total,
                    Data = checkSims
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<dynamic> CheckSimAsync(PtfScopingCheckSimRequest request)
        {
            try
            {
                var queryCredit = QueryCreditScoreAsync(request.MobileNetwork, request.GetPhoneNumber(), request.IdCard);
                // var queryFraud = QueryFraudScoreAsync(request.MobileNetwork, request.GetPhoneNumber(), request.IdCard);

                // await Task.WhenAll(queryCredit, queryFraud);

                return new
                {
                    QueryCredit = await queryCredit,
                    // QueryFraud = await queryFraud,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<dynamic> SendOtpAsync(PtfScopingSendOtpRequest request)
        {
            if (request.MobileNetwork == PtfScoringMobileNetwork.Viettel)
            {
                return await SendOtpViettelAsync(request);
            }
            else
            {
                return await SendOtpVNPTAsync(request);
            }
        }

        public async Task<dynamic> SendOtpViettelAsync(PtfScopingSendOtpRequest request)
        {
            try
            {
                var checkConsent = await CheckConsentAsync(request.GetPhoneNumber(), request.IdCard);
                // Kiểm tra checkconsent nếu có thì bỏ qua bước gửi OTP
                if (checkConsent != null && !string.IsNullOrEmpty(checkConsent.Data?.Data?.ConsentId))
                {
                    var queryCredit = FetchViettelCreditScoreAsync(request.GetPhoneNumber(), request.IdCard);
                    // var queryFraud = QueryFraudScoreAsync(request.MobileNetwork, request.GetPhoneNumber(), request.IdCard);
                    // await Task.WhenAll(queryCredit, queryFraud);
                    return new
                    {
                        CheckConsent = checkConsent,
                        QueryCredit = await queryCredit,
                        // QueryFraud = await queryFraud,
                    };
                }
                else
                {
                    var sendOtp = await SendOtpAsync(request.MobileNetwork, request.GetPhoneNumber(), request.IdCard);
                    return new
                    {
                        SendOtp = sendOtp,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<dynamic> SendOtpVNPTAsync(PtfScopingSendOtpRequest request)
        {
            try
            {
                var checkConsent = await CheckMnpAsync(request.GetPhoneNumber(), request.IdCard);
                // Kiểm tra checkconsent nếu có thì tiếp tục check ErrorCode
                if (checkConsent != null && checkConsent.Success)
                {
                    if (checkConsent.Data?.Data?.Mnp == 0)
                    {
                        throw new ArgumentException("SĐT không hợp lệ/không thỏa chính sách của VNPT");
                    }
                    else
                    {
                        var queryCredit = await FetchVnptCreditScoreAsync(request.GetPhoneNumber(), request.IdCard);
                        // var queryFraud = QueryFraudScoreAsync(request.MobileNetwork, request.GetPhoneNumber(), request.IdCard);
                        // await Task.WhenAll(queryCredit, queryFraud);

                        // Nếu ErrorCode là 109 thì mới gửi OTP, còn lại thì Fetch data về
                        if (queryCredit?.Data?.Data?.ErrorCode == 109)
                        {
                            var sendOtp = await SendOtpAsync(request.MobileNetwork, request.GetPhoneNumber(), request.IdCard);
                            return new
                            {
                                SendOtp = sendOtp,
                            };
                        }
                        return new
                        {
                            CheckConsent = checkConsent,
                            QueryCredit = queryCredit,
                            // QueryFraud = await queryFraud,
                        };
                    }
                }
                else
                {
                    throw new ArgumentException("Số điện thoại không thuộc nhà mạng VNPT");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<dynamic> VerifyOtpAsync(PtfScopingVerifyOtpRequest request)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var checkSim = await _checkSimRepository.FindLastAsync(request.IdCard, request.GetPhoneNumber(), userId, CheckSimAction.SendOTP, CheckSimProject.PTF);
                if (checkSim == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(CheckSim)));
                }
                dynamic verifyOtp = null;
                if (request.MobileNetwork == PtfScoringMobileNetwork.Viettel)
                {
                    var sendOtpResponse = JsonConvert.DeserializeObject<PtfScoringBaseRestResponse<PtfScoringSendOtpRestResponse>>(checkSim.Response);
                    var requestId = sendOtpResponse?.Data?.Data?.RequestId;
                    verifyOtp = await VerifyOtpViettelAsync(request.Otp, requestId, request.GetPhoneNumber(), request.IdCard);
                }
                else
                {
                    verifyOtp = await VerifyOtpVnptAsync(request.Otp, request.GetPhoneNumber(), request.IdCard);
                }
                if (verifyOtp != null && verifyOtp.success == true)
                {
                    dynamic fetchCreditScore = null;
                    if (request.MobileNetwork == PtfScoringMobileNetwork.Viettel)
                    {
                        fetchCreditScore = FetchViettelCreditScoreAsync(request.GetPhoneNumber(), request.IdCard);
                    }
                    else
                    {
                        fetchCreditScore = FetchVnptCreditScoreAsync(request.GetPhoneNumber(), request.IdCard);
                    }
                    // var fetchFraudScore = FetchFraudScoreAsync(request.MobileNetwork, request.GetPhoneNumber(), request.IdCard, request.FrequentContacts);

                    // await Task.WhenAll(fetchCreditScore, fetchFraudScore);
                    return new
                    {
                        VerifyOtp = verifyOtp,
                        FetchCreditScore = await fetchCreditScore,
                        // FetchFraudScore = await fetchFraudScore,
                    };
                }
                else
                {
                    return new
                    {
                        VerifyOtp = verifyOtp,
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        #region PTF Scoring

        private async Task<dynamic> QueryCreditScoreAsync(PtfScoringMobileNetwork mobileNetwork, string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringTrustingSocialRestService), nameof(IPtfScoringTrustingSocialRestService.QueryCreditScoreAsync)) :
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringVnptRestService), nameof(IPtfScoringVnptRestService.QueryCreditScoreAsync));
            var request = new PtfScoringQueryCreditScoreRestRequest { PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                MobileNetwork = mobileNetwork.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.QueryCreditScore,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                    await _ptfScoringTrustingSocialRestService.QueryCreditScoreAsync(request) :
                    await _ptfScoringVnptRestService.QueryCreditScoreAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<dynamic> QueryFraudScoreAsync(PtfScoringMobileNetwork mobileNetwork, string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringTrustingSocialRestService), nameof(IPtfScoringTrustingSocialRestService.QueryFraudScoreAsync)) :
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringVnptRestService), nameof(IPtfScoringVnptRestService.QueryFraudScoreAsync));
            var request = new PtfScoringQueryFraudScoreRestRequest { PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                MobileNetwork = mobileNetwork.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.QueryFraudScore,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                    await _ptfScoringTrustingSocialRestService.QueryFraudScoreAsync(request) :
                    await _ptfScoringVnptRestService.QueryFraudScoreAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<PtfScoringBaseRestResponse<PtfScoringCheckConsentRestResponse>> CheckConsentAsync(string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfScoringTrustingSocialRestService), nameof(IPtfScoringTrustingSocialRestService.CheckConsentAsync));
            var request = new PtfScoringCheckConsentRestRequest { PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                MobileNetwork = PtfScoringMobileNetwork.Viettel.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.CheckConsent,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfScoringTrustingSocialRestService.CheckConsentAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<PtfScoringBaseRestResponse<PtfScoringCheckMnpRestResponse>> CheckMnpAsync(string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfScoringVnptRestService), nameof(IPtfScoringVnptRestService.CheckMnpAsync));
            var request = new PtfScoringCheckConsentRestRequest { PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                MobileNetwork = PtfScoringMobileNetwork.VNPT.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.CheckConsent,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfScoringVnptRestService.CheckMnpAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }
        private async Task<dynamic> SendOtpAsync(PtfScoringMobileNetwork mobileNetwork, string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringTrustingSocialRestService), nameof(IPtfScoringTrustingSocialRestService.SendOtpAsync)) :
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringVnptRestService), nameof(IPtfScoringVnptRestService.SendOtpAsync));
            var request = new PtfScoringSendOtpRestRequest { PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                MobileNetwork = mobileNetwork.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.SendOTP,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                    await _ptfScoringTrustingSocialRestService.SendOtpAsync(request) :
                    await _ptfScoringVnptRestService.SendOtpAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<dynamic> VerifyOtpViettelAsync(string otp, string requestId, string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfScoringTrustingSocialRestService), nameof(IPtfScoringTrustingSocialRestService.VerifyOtpAsync));
            var request = new PtfScoringVerifyOtpRestRequest { Otp = otp, RequestId = requestId };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                OTP = otp,
                PhoneNumber = phoneNumber,
                MobileNetwork = PtfScoringMobileNetwork.Viettel.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.VerifyOTP,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfScoringTrustingSocialRestService.VerifyOtpAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<dynamic> VerifyOtpVnptAsync(string otp, string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfScoringVnptRestService), nameof(IPtfScoringVnptRestService.VerifyOtpAsync));
            var request = new PtfScoringVerifyOtpVnptRestRequest { Otp = otp, PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                OTP = otp,
                PhoneNumber = phoneNumber,
                MobileNetwork = PtfScoringMobileNetwork.VNPT.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.VerifyOTP,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfScoringVnptRestService.VerifyOtpAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<dynamic> FetchViettelCreditScoreAsync(string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfScoringTrustingSocialRestService), nameof(IPtfScoringTrustingSocialRestService.FetchCreditScoreAsync));
            var request = new PtfScoringFetchCreditScoreRestRequest { PhoneNumber = phoneNumber, IdType = "national_id", IdValue = idCard };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                MobileNetwork = PtfScoringMobileNetwork.Viettel.ToString(),
                IdCard = idCard,
                Action = CheckSimAction.FetchCreditScore,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfScoringTrustingSocialRestService.FetchCreditScoreAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<dynamic> FetchVnptCreditScoreAsync(string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfScoringVnptRestService), nameof(IPtfScoringVnptRestService.FetchCreditScoreAsync));
            var request = new PtfScoringFetchVNPTCreditScoreRestRequest { PhoneNumber = phoneNumber, NationalId = idCard };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                MobileNetwork = PtfScoringMobileNetwork.VNPT.ToString(),
                PhoneNumber = phoneNumber,
                IdCard = idCard,
                Action = CheckSimAction.FetchCreditScore,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfScoringVnptRestService.FetchCreditScoreAsync(request);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<PtfScoringBaseRestResponse<PtfScoringFetchCreditVpntRestResponse>>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        private async Task<dynamic> FetchFraudScoreAsync(PtfScoringMobileNetwork mobileNetwork, string phoneNumber, string idCard, IEnumerable<string> frequentContacts)
        {
            var ptfScoringTrustingSocialFetchFraudScoreRestRequest = new PtfScoringTrustingSocialFetchFraudScoreRestRequest
            {
                PhoneNumber = phoneNumber,
                IdType = "national_id",
                IdValue = idCard,
                FrequentContacts = frequentContacts
            };
            var ptfScoringVnptFetchFraudScoreRestRequest = new PtfScoringVnptFetchFraudScoreRestRequest
            {
                PhoneNumber = phoneNumber,
                IdType = "national_id",
                IdValue = idCard,
                FrequentContactsVnptLending = frequentContacts
            };

            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringTrustingSocialRestService), nameof(IPtfScoringTrustingSocialRestService.FetchFraudScoreAsync)) :
                HelperExtension.GetHttpMethodPath(typeof(IPtfScoringVnptRestService), nameof(IPtfScoringVnptRestService.FetchFraudScoreAsync));

            var payload = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                JsonConvert.SerializeObject(ptfScoringTrustingSocialFetchFraudScoreRestRequest) :
                JsonConvert.SerializeObject(ptfScoringVnptFetchFraudScoreRestRequest);

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                IdCard = idCard,
                Action = CheckSimAction.FetchFraudScore,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = payload
            };
            dynamic result = null;

            try
            {
                result = mobileNetwork == PtfScoringMobileNetwork.Viettel ?
                    await _ptfScoringTrustingSocialRestService.FetchFraudScoreAsync(ptfScoringTrustingSocialFetchFraudScoreRestRequest) :
                    await _ptfScoringVnptRestService.FetchFraudScoreAsync(ptfScoringVnptFetchFraudScoreRestRequest);

                checkSim.Response = JsonConvert.SerializeObject(result);
            }
            catch (ApiException ex)
            {
                checkSim.Message = ex.Message;
                checkSim.Response = ex.Content;
                result = JsonConvert.DeserializeObject<object>(ex.Content);
            }

            await _checkSimRepository.InsertOneAsync(checkSim);
            return result;
        }

        #endregion

        private async Task<(Sale, TeamLeadInfo, PosInfo, SaleChanelInfo, TeamLeadInfo)> GetManagerMetaDataAsync()
        {
            var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
            var saleInfo = _mapper.Map<Sale>(user);
            return (saleInfo, user.TeamLeadInfo, user.PosInfo, user.SaleChanelInfo, user.AsmInfo);
        }
    }
}
