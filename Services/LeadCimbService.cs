using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadCimbs;
using _24hplusdotnetcore.ModelDtos.Otps;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Settings;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadCimbService
    {
        Task<CreateLeadCimbResponse> CreateAsync(CreateLeadCimbRequest createLeadCimbRequest);
        Task<CimbSendVerifyResponse> SendVerifyAsync(string id, CimbSendVerifyRequest cimbSendVerifyRequest);
        Task<string> VerifyAsync(string id, CimbVerifyRequest cimbVerifyRequest);
        Task<UpdateLeadCimbStep1Response> UpdateStep1Async(string id, UpdateLeadCimbStep1Request updateLeadCimbStep1Request);
        Task UpdateStep2Async(string id, UpdateLeadCimbStep2Request updateLeadCimbStep2Request);
        Task UpdateStep3Async(string id, UpdateLeadCimbStep3Request updateLeadCimbStep3Request);
        Task UpdateStep4Async(string id, UpdateLeadCimbStep4Request updateLeadCimbStep4Request);
        Task<GetLeadCimbDetailResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
        Task<PagingResponse<GetLeadCimbResponse>> GetAsync(GetLeadCimbRequest getLeadCimbRequest);
        Task SubmitAsync(string id);
        Task<IEnumerable<GetCimbCityResponse>> GetCitiesAsync();
        Task<IEnumerable<GetCimbDistrictResponse>> GetDistrictAsync(string stateId);
        Task<IEnumerable<GetCimbWardResponse>> GetWardAsync(string cityId);
        Task<IEnumerable<LeadCimbLoanInfomationResponse>> GetLoanInfomationAsync(LeadCimbLoanInfomationRequest request);
    }

    public class LeadCimbService : ILeadCimbService, IScopedLifetime
    {
        private readonly ILogger<LeadCimbService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly ICustomerRepository _customerRepository;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<DataCimbProcessing> _dataCimbProcessingRepository;
        private readonly IMongoRepository<LeadCimbResource> _leadCimbResourceRepository;
        private readonly IMongoRepository<LeadCimbVerifyOtp> _leadCimbVerifyOtpRepository;
        private readonly IMongoRepository<LeadCimbLoanInfomation> _leadCimbLoanInfomationRepository;
        private readonly IOtpRestService _otpRestService;
        private readonly OtpConfig _otpConfig;
        private readonly IMongoRepository<LeadCimbVerifyOtpHistory> _leadCimbVerifyOtpHistoryRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userServices;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadCimbService(
            ILogger<LeadCimbService> logger,
            IMapper mapper,
            IUserLoginService userLoginService,
            ICustomerRepository customerRepository,
            DataCRMProcessingServices dataCRMProcessingServices,
            IMongoRepository<DataCimbProcessing> dataCimbProcessingRepository,
            IMongoRepository<LeadCimbResource> leadCimbResourceRepository,
            IMongoRepository<LeadCimbVerifyOtp> leadCimbVerifyOtpRepository,
            IMongoRepository<LeadCimbLoanInfomation> leadCimbLoanInfomationRepository,
            IOtpRestService otpRestService,
            IOptions<OtpConfig> otpConfigOption,
            IMongoRepository<LeadCimbVerifyOtpHistory> leadCimbVerifyOtpHistoryRepository,
            IHistoryDomainService historyDomainService,
            IUserRepository userRepository,
            IUserServices userServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _customerRepository = customerRepository;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _dataCimbProcessingRepository = dataCimbProcessingRepository;
            _leadCimbResourceRepository = leadCimbResourceRepository;
            _leadCimbVerifyOtpRepository = leadCimbVerifyOtpRepository;
            _leadCimbLoanInfomationRepository = leadCimbLoanInfomationRepository;
            _otpRestService = otpRestService;
            _otpConfig = otpConfigOption.Value;
            _leadCimbVerifyOtpHistoryRepository = leadCimbVerifyOtpHistoryRepository;
            _historyDomainService = historyDomainService;
            _userRepository = userRepository;
            _userServices = userServices;
            _posRepository = posRepository;
        }

        public async Task<CreateLeadCimbResponse> CreateAsync(CreateLeadCimbRequest createLeadCimbRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createLeadCimbRequest.Personal.IdCard, createLeadCimbRequest.Personal.Email, createLeadCimbRequest.Personal.Phone);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createLeadCimbRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.Creator = _userLoginService.GetUserId();
                customer.ProductLine = "TSA";
                customer.Documents = new List<GroupDocument>
                {
                    new GroupDocument
                    {
                        GroupId = 1,
                        GroupName = "Hình ảnh Khách hàng",
                        Mandatory = true,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                DocumentName = "Hình ảnh Khách hàng",
                                DocumentCode = "SELFIE"
                            }
                        }
                    },
                    new GroupDocument
                    {
                        GroupId = 2,
                        GroupName = "CMND mặt trước",
                        Mandatory = true,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                DocumentName = "CMND mặt trước",
                                DocumentCode = "CERT_FRONT_PIC"
                            }
                        }
                    },
                    new GroupDocument
                    {
                        GroupId = 3,
                        GroupName = "CMND mặt sau",
                        Mandatory = true,
                        Documents = new List<DocumentUpload>
                        {
                            new DocumentUpload
                            {
                                DocumentName = "CMND mặt sau",
                                DocumentCode = "CERT_BACK_PIC"
                            }
                        }
                    }
                };

                var leadCimbVerifyOtp = await _leadCimbVerifyOtpRepository.FindOneAsync(x =>
                    x.UserId == _userLoginService.GetUserId() &&
                    x.CustomerEmail == customer.Personal.Email &&
                    x.CustomerPhone == customer.Personal.Phone &&
                    x.CreatedDate > DateTime.Now.AddDays(-30));
                customer.Personal.IsEmailVerified = leadCimbVerifyOtp?.IsEmailVerified ?? false;
                customer.Personal.IsPhoneVerified = leadCimbVerifyOtp?.IsPhoneVerified ?? false;

                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                var response = _mapper.Map<CreateLeadCimbResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CimbSendVerifyResponse> SendVerifyAsync(string id, CimbSendVerifyRequest cimbSendVerifyRequest)
        {
            LeadCimbVerifyOtpHistory leadCimbVerifyOtpHistory = new LeadCimbVerifyOtpHistory();
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (string.IsNullOrEmpty(customer.Personal.IdCard))
                {
                    throw new ArgumentException(string.Format(Message.COMMON_REQUIRED, nameof(Personal.IdCard)));
                }

                if (cimbSendVerifyRequest.Type == VerifyType.Email && string.IsNullOrEmpty(customer.Personal.Email))
                {
                    throw new ArgumentException(string.Format(Message.COMMON_REQUIRED, nameof(Personal.Email)));
                }

                if (cimbSendVerifyRequest.Type == VerifyType.Phone && string.IsNullOrEmpty(customer.Personal.Phone))
                {
                    throw new ArgumentException(string.Format(Message.COMMON_REQUIRED, nameof(Personal.Phone)));
                }

                var otpValue = cimbSendVerifyRequest.Type == VerifyType.Email ? customer.Personal.Email : customer.Personal.Phone;

                var history = await _leadCimbVerifyOtpHistoryRepository.FindOneAsync(x =>
                    x.Creator == _userLoginService.GetUserId() &&
                    x.Type == $"{cimbSendVerifyRequest.Type}" &&
                    x.Value == otpValue &&
                    x.IsSendSuccess == true &&
                    x.Action == HistoryActionType.Send &&
                    x.CreatedDate > DateTime.Now.AddMinutes(-3));
                if (history != null)
                {
                    throw new ArgumentException("OTP đã được gửi gần đây, vui lòng kiểm tra và thử lại sau 3 phút!");
                }

                SendRestRequest sendRestRequest = new SendRestRequest
                {
                    Fullname = customer.Personal.Name,
                    Identifier = customer.Personal.IdCard,
                    Type = cimbSendVerifyRequest.Type ?? VerifyType.Email,
                    Value = otpValue
                };

                leadCimbVerifyOtpHistory.CustomerId = customer.Id;
                leadCimbVerifyOtpHistory.Type = $"{sendRestRequest.Type}";
                leadCimbVerifyOtpHistory.Value = sendRestRequest.Value;
                leadCimbVerifyOtpHistory.Action = HistoryActionType.Send;
                leadCimbVerifyOtpHistory.PayLoad = JsonConvert.SerializeObject(sendRestRequest);
                leadCimbVerifyOtpHistory.Creator = _userLoginService.GetUserId();

                SendRestResponse response = new SendRestResponse();
                if (_otpConfig.IsTestMode)
                {
                    response = new SendRestResponse { Message = "Test mode! Bạn có thể verify bằng mã: 123456" };
                }
                else
                {
                    response = await _otpRestService.SendAsync(sendRestRequest);
                }

                if (!string.IsNullOrEmpty(response.Results?.VerifiedOtp) && response.Results.Code == StatusCodes.Status200OK.ToString())
                {
                    var valueBefore = customer.Clone();

                    customer.Personal.IsEmailVerified = cimbSendVerifyRequest.Type == VerifyType.Email ? true : customer.Personal.IsEmailVerified;
                    customer.Personal.IsPhoneVerified = cimbSendVerifyRequest.Type == VerifyType.Phone ? true : customer.Personal.IsPhoneVerified;
                    customer.Modifier = _userLoginService.GetUserId();
                    customer.ModifiedDate = DateTime.Now;

                    await _customerRepository.ReplaceOneAsync(customer);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SendVerifyAsync), valueBefore, customer);
                }

                leadCimbVerifyOtpHistory.Response = JsonConvert.SerializeObject(response);
                leadCimbVerifyOtpHistory.IsSendSuccess = true;
                await _leadCimbVerifyOtpHistoryRepository.InsertOneAsync(leadCimbVerifyOtpHistory);

                var result = _mapper.Map<CimbSendVerifyResponse>(customer);
                result.Message = response.Message;
                return result;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<SendRestResponse>();
                leadCimbVerifyOtpHistory.Response = JsonConvert.SerializeObject(error);
                await _leadCimbVerifyOtpHistoryRepository.InsertOneAsync(leadCimbVerifyOtpHistory);
                throw new ArgumentException(error.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<string> VerifyAsync(string id, CimbVerifyRequest cimbVerifyRequest)
        {
            LeadCimbVerifyOtpHistory verifyHistory = new LeadCimbVerifyOtpHistory();
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var valueBefore = customer.Clone();

                VerifyRestRequest verifyRestRequest = new VerifyRestRequest
                {
                    Fullname = customer.Personal.Name,
                    Identifier = customer.Personal.IdCard,
                    Type = $"{cimbVerifyRequest.Type}",
                    Value = cimbVerifyRequest.Type == VerifyType.Email ? customer.Personal.Email : customer.Personal.Phone,
                    Otp = cimbVerifyRequest.Token
                };

                verifyHistory.CustomerId = customer.Id;
                verifyHistory.Type = verifyRestRequest.Type;
                verifyHistory.Value = verifyRestRequest.Value;
                verifyHistory.Action = HistoryActionType.Verify;
                verifyHistory.PayLoad = JsonConvert.SerializeObject(verifyRestRequest);
                verifyHistory.Creator = _userLoginService.GetUserId();

                var response = new VerifyRestResponse();
                if (_otpConfig.IsTestMode && cimbVerifyRequest.Token == "123456")
                {
                    response.Message = "OK";
                }
                else
                {
                    response = await _otpRestService.VerifyAsync(verifyRestRequest);
                }

                verifyHistory.Response = JsonConvert.SerializeObject(response);
                verifyHistory.IsSendSuccess = true;
                await _leadCimbVerifyOtpHistoryRepository.InsertOneAsync(verifyHistory);

                customer.Personal.IsEmailVerified = cimbVerifyRequest.Type == VerifyType.Email ? true : customer.Personal.IsEmailVerified;
                customer.Personal.IsPhoneVerified = cimbVerifyRequest.Type == VerifyType.Phone ? true : customer.Personal.IsPhoneVerified;
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(VerifyAsync), valueBefore, customer);


                var leadCimbVerifyOtp = await _leadCimbVerifyOtpRepository.FindOneAsync(x =>
                    x.UserId == _userLoginService.GetUserId() &&
                    x.CustomerEmail == customer.Personal.Email &&
                    x.CustomerPhone == customer.Personal.Phone &&
                    x.CreatedDate > DateTime.Now.AddDays(-30));
                if (leadCimbVerifyOtp != null)
                {
                    var leadCimbVerifyBefore = customer.Clone();

                    leadCimbVerifyOtp.IsEmailVerified = cimbVerifyRequest.Type == VerifyType.Email ? true : leadCimbVerifyOtp.IsEmailVerified;
                    leadCimbVerifyOtp.IsPhoneVerified = cimbVerifyRequest.Type == VerifyType.Phone ? true : leadCimbVerifyOtp.IsPhoneVerified;
                    await _leadCimbVerifyOtpRepository.ReplaceOneAsync(leadCimbVerifyOtp);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(LeadCimbVerifyOtp), AuditActionType.Update, nameof(VerifyAsync), leadCimbVerifyBefore, leadCimbVerifyOtp);

                }
                else
                {
                    leadCimbVerifyOtp = new LeadCimbVerifyOtp
                    {
                        UserId = _userLoginService.GetUserId(),
                        CustomerName = customer.Personal.Name,
                        CustomerPhone = customer.Personal.Phone,
                        CustomerEmail = customer.Personal.Email,
                        CustomerIdCard = customer.Personal.IdCard,
                        IsPhoneVerified = cimbVerifyRequest.Type == VerifyType.Phone ? true : false,
                        IsEmailVerified = cimbVerifyRequest.Type == VerifyType.Email ? true : false
                    };
                    await _leadCimbVerifyOtpRepository.InsertOneAsync(leadCimbVerifyOtp);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(LeadCimbVerifyOtp), AuditActionType.Create, nameof(VerifyAsync), valueAfter: leadCimbVerifyOtp);
                }

                return response.Message;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<VerifyRestResponse>();
                verifyHistory.Response = JsonConvert.SerializeObject(error);
                await _leadCimbVerifyOtpHistoryRepository.InsertOneAsync(verifyHistory);
                throw new ArgumentException(error.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<UpdateLeadCimbStep1Response> UpdateStep1Async(string id, UpdateLeadCimbStep1Request updateLeadCimbStep1Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateLeadCimbStep1Request.Personal.IdCard, updateLeadCimbStep1Request.Personal.Email, updateLeadCimbStep1Request.Personal.Phone, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN, CustomerStatus.REJECT };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                if (!string.Equals(customer.Personal.Phone, updateLeadCimbStep1Request.Personal.Phone))
                {
                    customer.Personal.IsPhoneVerified = false;
                }
                if (!string.Equals(customer.Personal.Email, updateLeadCimbStep1Request.Personal.Email))
                {
                    customer.Personal.IsEmailVerified = false;
                }

                _mapper.Map(updateLeadCimbStep1Request, customer);

                var leadCimbVerifyOtp = await _leadCimbVerifyOtpRepository.FindOneAsync(x =>
                    x.UserId == _userLoginService.GetUserId() &&
                    x.CustomerEmail == customer.Personal.Email &&
                    x.CustomerPhone == customer.Personal.Phone &&
                    x.CreatedDate > DateTime.Now.AddDays(-30));
                customer.Personal.IsEmailVerified = leadCimbVerifyOtp?.IsEmailVerified ?? customer.Personal.IsEmailVerified;
                customer.Personal.IsPhoneVerified = leadCimbVerifyOtp?.IsPhoneVerified ?? customer.Personal.IsPhoneVerified;


                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep1Async), valueBefore, customer);

                var response = _mapper.Map<UpdateLeadCimbStep1Response>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep2Async(string id, UpdateLeadCimbStep2Request updateLeadCimbStep2Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(customer.Personal.IdCard, customer.Personal.Email, customer.Personal.Phone, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN, CustomerStatus.REJECT };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadCimbStep2Request, customer);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep2Async), valueBefore, customer);

                await _customerRepository.ReplaceOneAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep3Async(string id, UpdateLeadCimbStep3Request updateLeadCimbStep3Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(customer.Personal.IdCard, customer.Personal.Email, customer.Personal.Phone, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN, CustomerStatus.REJECT };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadCimbStep3Request, customer);
                customer.Loan.InterestRatePerYear = await this.GetLoanInterestRateAsync(customer.Loan.Amount, customer.Loan.TermId);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep3Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep4Async(string id, UpdateLeadCimbStep4Request updateLeadCimbStep4Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(customer.Personal.IdCard, customer.Personal.Email, customer.Personal.Phone, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN, CustomerStatus.REJECT };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadCimbStep4Request, customer);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep4Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetLeadCimbDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadCimbManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadCimbManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadCimbManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadCimbManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadCimbManagement_ViewAll);

                Expression<Func<Customer, bool>> filter = x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    (!filterByCreatorIds.Any() || filterByCreatorIds.Contains(x.Creator));
                var customer = await _customerRepository.FindOneAsync(filter);

                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var result = _mapper.Map<GetLeadCimbDetailResponse>(customer);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var valueBefore = customer.Clone();

                customer.IsDeleted = true;
                customer.DeletedBy = _userLoginService.GetUserId();
                customer.DeletedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(DeleteAsync), valueBefore, customer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetLeadCimbResponse>> GetAsync(GetLeadCimbRequest getLeadCimbRequest)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadCimbManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadCimbManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadCimbManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadCimbManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadCimbManagement_ViewAll);

                var customerFilter = new CustonerFilterDto
                {
                    GreenType = GreenType.GreenG,
                    CreatorIds = filterByCreatorIds,
                    Status = getLeadCimbRequest.Status,
                    CustomerName = getLeadCimbRequest.CustomerName,
                    FromDate = getLeadCimbRequest.GetFromDate(),
                    ToDate = getLeadCimbRequest.GetToDate(),
                    ReturnStatus = getLeadCimbRequest.ReturnStatus,
                    PageIndex = getLeadCimbRequest.PageIndex,
                    PageSize = getLeadCimbRequest.PageSize
                };
                var cimbs = await _customerRepository.GetAsync<GetLeadCimbResponse>(customerFilter);
                var total = await _customerRepository.CountAsync(customerFilter);

                var result = new PagingResponse<GetLeadCimbResponse>
                {
                    TotalRecord = total,
                    Data = cimbs
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SubmitAsync(string id)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN, CustomerStatus.REJECT };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var hasExiested = await IsExistedAsync(customer.Personal.IdCard, customer.Personal.Email, customer.Personal.Phone, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var valueBefore = customer.Clone();


                var validator = new CimbCustomerValidator();
                var result = validator.Validate(customer);

                if (!result.IsValid)
                {
                    customer.Status = CustomerStatus.RETURN;
                    customer.Result.Reason = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
                    customer.Modifier = _userLoginService.GetUserId();
                    customer.ModifiedDate = DateTime.Now;
                    await _customerRepository.ReplaceOneAsync(customer);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitAsync), valueBefore, customer);

                    throw new ArgumentException(string.Format(Message.COMMON_REQUIRED, customer.Result.Reason));
                }

                customer.Status = CustomerStatus.SUBMIT;
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitAsync), valueBefore, customer);

                await _dataCimbProcessingRepository.InsertOneAsync(new DataCimbProcessing(customer.Id));
                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    LeadSource = LeadSourceType.CIMB
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GetCimbCityResponse>> GetCitiesAsync()
        {
            try
            {
                var resources = _leadCimbResourceRepository.FilterBy(x => x.Type == $"{LeadCimbResourceType.CITY}");
                var result = _mapper.Map<IEnumerable<GetCimbCityResponse>>(resources);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GetCimbDistrictResponse>> GetDistrictAsync(string stateId)
        {
            try
            {
                Expression<Func<LeadCimbResource, bool>> filter = x => x.Type == $"{LeadCimbResourceType.DISTRICT}";
                if (!string.IsNullOrEmpty(stateId))
                {
                    filter = x => x.ParentCode == stateId;
                }
                var resources = _leadCimbResourceRepository.FilterBy(filter);
                var result = _mapper.Map<IEnumerable<GetCimbDistrictResponse>>(resources);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GetCimbWardResponse>> GetWardAsync(string cityId)
        {
            try
            {
                Expression<Func<LeadCimbResource, bool>> filter = x => x.Type == $"{LeadCimbResourceType.WARD}";
                if (!string.IsNullOrEmpty(cityId))
                {
                    filter = x => x.ParentCode == cityId;
                }
                var resources = _leadCimbResourceRepository.FilterBy(filter);
                var result = _mapper.Map<IEnumerable<GetCimbWardResponse>>(resources);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LeadCimbLoanInfomationResponse>> GetLoanInfomationAsync(LeadCimbLoanInfomationRequest request)
        {
            try
            {
                var loanInfomation = await _leadCimbLoanInfomationRepository.FindOneAsync(x => x.MinAmount < request.Amount && (!x.MaxAmount.HasValue || x.MaxAmount >= request.Amount));
                if (loanInfomation == null)
                {
                    return new List<LeadCimbLoanInfomationResponse>();
                }
                var result = loanInfomation?.Details?.Select(x => new LeadCimbLoanInfomationResponse(x.NumberOfMonth, x.InterestRatePerYear, x.MonthlyPaymentAmount(request.Amount)));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<bool> IsExistedAsync(string idCard, string email, string phone, string id = null)
        {
            DateTime datefrom = DateTime.Now.AddDays(-15);
            string[] listOfStatus = { CustomerStatus.SUBMIT, CustomerStatus.PROCESSING, CustomerStatus.SENDING };
            var customerExisted = await _customerRepository.FindOneAsync(x =>
                    !x.IsDeleted &&
                    listOfStatus.Contains(x.Status) &&
                    x.Id != id &&
                    x.ModifiedDate > datefrom &&
                    x.GreenType == GreenType.GreenG &&
                    (x.Personal.IdCard == idCard || x.Personal.Phone == phone || x.Personal.Email == email)
                    );
            return customerExisted != null;

        }

        private bool IsValid(Customer customer, out IList<string> propertieNames)
        {
            propertieNames = new List<string>();
            var requiredFields = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.Name)}", "Họ và tên"),
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.IdCard)}", "Số CMND/CCCD"),
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.DateOfBirth)}", "Ngày sinh"),
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.Gender)}", "Giới tính"),
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.Phone)}", "SĐT"),
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.Email)}", "Email"),
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.MaritalStatusId)}", "Tình trạng hôn nhân"),
                new KeyValuePair<string, string>($"{nameof(Customer.Personal)}.{nameof(Personal.EducationLevelId)}", "Học vấn"),

                new KeyValuePair<string, string>($"{nameof(Customer.Working)}.{nameof(Working.CompanyName)}", "Tên công ty"),
                new KeyValuePair<string, string>($"{nameof(Customer.Working)}.{nameof(Working.EmploymentStatusId)}", "Hình thức làm việc"),
                new KeyValuePair<string, string>($"{nameof(Customer.Working)}.{nameof(Working.Income)}", "Mức thu nhập VNĐ/tháng"),

                new KeyValuePair<string, string>($"{nameof(Customer.Referees)}.0.{nameof(Referee.Name)}", "Tên người tham chiếu 1"),
                new KeyValuePair<string, string>($"{nameof(Customer.Referees)}.0.{nameof(Referee.Phone)}", "SĐT người tham chiếu 1"),
                new KeyValuePair<string, string>($"{nameof(Customer.Referees)}.0.{nameof(Referee.RelationshipId)}", "Mối quan hệ người tham chiếu 1"),
                new KeyValuePair<string, string>($"{nameof(Customer.Referees)}.1.{nameof(Referee.Name)}", "Tên người tham chiếu 2"),
                new KeyValuePair<string, string>($"{nameof(Customer.Referees)}.1.{nameof(Referee.Phone)}", "SĐT người tham chiếu 2"),
                new KeyValuePair<string, string>($"{nameof(Customer.Referees)}.1.{nameof(Referee.RelationshipId)}", "Mối quan hệ người tham chiếu 2"),

                new KeyValuePair<string, string>($"{nameof(Customer.ResidentAddress)}.{nameof(Address.ProvinceId)}", "Tỉnh/Thành phố"),
                new KeyValuePair<string, string>($"{nameof(Customer.ResidentAddress)}.{nameof(Address.DistrictId)}", "Quận/huyện"),
                new KeyValuePair<string, string>($"{nameof(Customer.ResidentAddress)}.{nameof(Address.WardId)}", "Phường/xã"),
                new KeyValuePair<string, string>($"{nameof(Customer.ResidentAddress)}.{nameof(Address.Street)}", "Tên đường, số nhà"),

                new KeyValuePair<string, string>($"{nameof(Customer.Loan)}.{nameof(Loan.Amount)}", "Số tiền đề nghị vay"),
                new KeyValuePair<string, string>($"{nameof(Customer.Loan)}.{nameof(Loan.TermId)}", "Kỳ hạn vay"),
                new KeyValuePair<string, string>($"{nameof(Customer.Loan)}.{nameof(Loan.PurposeId)}", "Mục đích vay"),

                new KeyValuePair<string, string>($"{nameof(Customer.Documents)}.0.{nameof(GroupDocument.Documents)}.0.{nameof(DocumentUpload.UploadedMedias)}.0.{nameof(UploadedMedia.Uri)}", "Hình ảnh Khách hàng"),
                new KeyValuePair<string, string>($"{nameof(Customer.Documents)}.1.{nameof(GroupDocument.Documents)}.0.{nameof(DocumentUpload.UploadedMedias)}.0.{nameof(UploadedMedia.Uri)}", "CMND mặt trước"),
                new KeyValuePair<string, string>($"{nameof(Customer.Documents)}.2.{nameof(GroupDocument.Documents)}.0.{nameof(DocumentUpload.UploadedMedias)}.0.{nameof(UploadedMedia.Uri)}", "CMND mặt sau"),
            };

            foreach (var field in requiredFields)
            {
                string value = customer.GetDeepPropertyValue(field.Key)?.ToString();
                if (string.IsNullOrEmpty(value))
                {
                    propertieNames.Add(field.Value);
                }
            }
            return !propertieNames.Any();
        }
        private async Task<string> GetLoanInterestRateAsync(string amount, string term)
        {
            try
            {
                amount = amount.Replace(",", string.Empty);
                double.TryParse(amount, out double loanAmt);
                int.TryParse(term, out int ternNum);
                var loanInfomation = await _leadCimbLoanInfomationRepository.FindOneAsync(x => x.MinAmount < loanAmt && (!x.MaxAmount.HasValue || x.MaxAmount >= loanAmt));
                if (loanInfomation == null)
                {
                    return "";
                }
                var result = loanInfomation?.Details?.Where(x => x.NumberOfMonth == ternNum).FirstOrDefault();
                return result?.InterestRatePerYear.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return "";
            }
        }
    }
}
