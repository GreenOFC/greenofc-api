
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckSims;
using _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeDto;
using _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeRest;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.eWalletTransaction;
using _24hplusdotnetcore.Repositories.MC;
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

namespace _24hplusdotnetcore.Services.PtfOmnis
{
    public interface IPtfCheckIncomeService
    {
        Task<PagingResponse<GetCheckSimResponse>> GetAsync(GetCheckSimRequest request);
        Task<dynamic> QueryAsync(PtfQueryRequestDto request);
        Task<dynamic> SendOtpAsync(PtfQueryRequestDto request);
        Task<dynamic> VerifyOtpAsync(PtfVerifyOtpRequestDto request);
    }
    public class PtfCheckIncomeService : IPtfCheckIncomeService, IScopedLifetime
    {
        private readonly ILogger<PtfCheckIncomeService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICheckSimRepository _checkSimRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPtfCheckIncomeRestService _ptfCheckIncomeRestService;
        private readonly PtfScoringConfig _ptfScoringConfig;
        private readonly IUserServices _userService;

        public PtfCheckIncomeService(
            ILogger<PtfCheckIncomeService> logger,
            IMapper mapper,
            IUserLoginService userLoginService,
            ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            IOptions<PtfScoringConfig> ptfScoringConfigOptions,
            ICheckSimRepository checkSimRepository,
            IPtfCheckIncomeRestService ptfCheckIncomeRestService,
            IUserServices userService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _ptfScoringConfig = ptfScoringConfigOptions.Value;
            _ptfCheckIncomeRestService = ptfCheckIncomeRestService;
            _checkSimRepository = checkSimRepository;
            _userService = userService;
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

                var checkSims = await _checkSimRepository.GetAsync(filterByCreatorIds, request);
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

        public async Task<dynamic> QueryAsync(PtfQueryRequestDto dto)
        {
            try
            {
                var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
                var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfCheckIncomeRestService), nameof(IPtfCheckIncomeRestService.QueryIncomeAsync));
                var requestBody = new PtfCheckIncomeCheckConsentRestRequest
                {
                    PhoneNumber = dto.GetPhoneNumber(),
                };

                var checkSim = new CheckSim
                {
                    Creator = _userLoginService.GetUserId(),
                    Project = CheckSimProject.PTF_CI,
                    SaleInfo = saleInfo,
                    TeamLeadInfo = teamLeadInfo,
                    AsmInfo = asmInfo,
                    PosInfo = posInfo,
                    SaleChanelInfo = saleChanelInfo,
                    PhoneNumber = dto.GetPhoneNumber(),
                    IdCard = dto.IdCard,
                    Action = CheckSimAction.CheckIncomeQuery,
                    AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                    Payload = JsonConvert.SerializeObject(requestBody)
                };
                dynamic result = null;

                try
                {
                    result = await _ptfCheckIncomeRestService.QueryIncomeAsync(requestBody);

                    checkSim.Response = JsonConvert.SerializeObject(result);
                }
                catch (ApiException ex)
                {
                    checkSim.Message = ex.Message;
                    checkSim.Response = ex.Content;
                    result = JsonConvert.DeserializeObject<object>(ex.Content);
                }

                await _checkSimRepository.InsertOneAsync(checkSim);
                return new
                {
                    Query = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<dynamic> SendOtpAsync(PtfQueryRequestDto request)
        {
            try
            {
                var checkConsent = await CheckConsentAsync(request.GetPhoneNumber(), request.IdCard);
                // Kiểm tra checkconsent nếu có thì bỏ qua bước gửi OTP
                if (checkConsent != null && checkConsent.Data != null && checkConsent.Data.ConsentId != 0)
                {
                    var fetch = await FetchCheckIncomeAsync(request.GetPhoneNumber(), request.IdCard);
                    return new
                    {
                        CheckConsent = checkConsent,
                        Fetch = fetch,
                    };
                }
                else
                {
                    var sendOtp = await SendOtpCheckIncomeAsync(request.GetPhoneNumber(), request.IdCard);
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

        public async Task<dynamic> VerifyOtpAsync(PtfVerifyOtpRequestDto request)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var checkSim = await _checkSimRepository.FindLastAsync(request.IdCard, request.GetPhoneNumber(), userId, CheckSimAction.CheckIncomeSendOTP, CheckSimProject.PTF_CI);
                if (checkSim == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(CheckSim)));
                }
                var sendOtpResponse = JsonConvert.DeserializeObject<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeSendOtpRestResponse>>(checkSim.Response);
                var verifyOtp = await VerifyOtpCheckIncomeAsync(request.Otp, sendOtpResponse.Data.OtpId.ToString(), request.GetPhoneNumber(), request.IdCard);

                if (verifyOtp != null && verifyOtp.Success == true)
                {
                    var fetch = await FetchCheckIncomeAsync(request.GetPhoneNumber(), request.IdCard);
                    return new
                    {
                        VerifyOtp = verifyOtp,
                        Fetch = fetch,
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

        private async Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeCheckConsentRestResponse>> CheckConsentAsync(string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfCheckIncomeRestService), nameof(IPtfCheckIncomeRestService.CheckConsentAsync));
            var request = new PtfCheckIncomeCheckConsentRestRequest { PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF_CI,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                IdCard = idCard,
                Action = CheckSimAction.CheckIncomeCheckConsent,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfCheckIncomeRestService.CheckConsentAsync(request);

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

        private async Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeSendOtpRestResponse>> SendOtpCheckIncomeAsync(string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfCheckIncomeRestService), nameof(IPtfCheckIncomeRestService.SendOtpAsync));
            var request = new PtfCheckIncomeSendOtpRestRequest { PhoneNumber = phoneNumber };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF_CI,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                IdCard = idCard,
                Action = CheckSimAction.CheckIncomeSendOTP,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfCheckIncomeRestService.SendOtpAsync(request);

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

        private async Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeVerifyOtpRestResponse>> VerifyOtpCheckIncomeAsync(string otp, string otpId, string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfCheckIncomeRestService), nameof(IPtfCheckIncomeRestService.VerifyOtpAsync));
            var request = new PtfCheckIncomeVerifyOtpRestRequest { Otp = otp, OtpId = otpId };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF_CI,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                OTP = otp,
                PhoneNumber = phoneNumber,
                IdCard = idCard,
                Action = CheckSimAction.CheckIncomeVerifyOTP,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfCheckIncomeRestService.VerifyOtpAsync(request);

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

        private async Task<dynamic> FetchCheckIncomeAsync(string phoneNumber, string idCard)
        {
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var methodPath = HelperExtension.GetHttpMethodPath(typeof(IPtfCheckIncomeRestService), nameof(IPtfCheckIncomeRestService.FetchIncomeAsync));
            var request = new PtfCheckIncomeFetchRestRequest { PhoneNumber = phoneNumber, IdCode = idCard };

            var checkSim = new CheckSim
            {
                Creator = _userLoginService.GetUserId(),
                Project = CheckSimProject.PTF_CI,
                SaleInfo = saleInfo,
                TeamLeadInfo = teamLeadInfo,
                AsmInfo = asmInfo,
                PosInfo = posInfo,
                SaleChanelInfo = saleChanelInfo,
                PhoneNumber = phoneNumber,
                IdCard = idCard,
                Action = CheckSimAction.CheckIncomeFetch,
                AbsolutePath = $"{_ptfScoringConfig.Host}{methodPath}",
                Payload = JsonConvert.SerializeObject(request)
            };
            dynamic result = null;

            try
            {
                result = await _ptfCheckIncomeRestService.FetchIncomeAsync(request);

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
