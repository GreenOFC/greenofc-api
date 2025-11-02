using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.EC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.EC;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Settings;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public class ECDataProcessingService : IScopedLifetime
    {
        private const string defaultProfession = "BOR";
        private readonly ILogger<ECDataProcessingService> _logger;
        private readonly IMapper _mapper;
        private readonly ECConfig _ecConfig;
        private readonly IECRestService _ecRestService;
        private readonly IMongoRepository<Customer> _customerCollection;
        private readonly ECAuthorizationService _ecAuthorizationService;
        private readonly ECNotificationService _ecNotificationService;
        private readonly IECDataProcessingRepository _ecDataProcessingRepository;
        private readonly IECOfferDataRepository _ecOfferDataRepository;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;

        private readonly IHistoryDomainService _historyDomainService;

        public ECDataProcessingService(
            ILogger<ECDataProcessingService> logger,
            IMapper mapper,
            IECRestService ecRestService,
             IOptions<ECConfig> ecConfig,
            ECAuthorizationService ecAuthorizationService,
            IMongoRepository<Customer> customerCollection,
            ECNotificationService ecNotificationService,
            IECDataProcessingRepository ecDataProcessingRepository,
            IECOfferDataRepository ecOfferDataRepository,
            DataCRMProcessingServices dataCRMProcessingServices,
            IHistoryDomainService historyDomainService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _ecRestService = ecRestService;
            _ecConfig = ecConfig.Value;
            _ecAuthorizationService = ecAuthorizationService;
            _customerCollection = customerCollection;
            _ecNotificationService = ecNotificationService;
            _ecDataProcessingRepository = ecDataProcessingRepository;
            _ecOfferDataRepository = ecOfferDataRepository;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _historyDomainService = historyDomainService;
        }

        public async Task CreateFullLoanPayload(string customerId, ECFullLoanDto request)
        {
            try
            {
                var serializedRequest = JsonConvert.SerializeObject(request);
                var body = new ECDataProcessing(customerId);
                body.PayLoad = serializedRequest;

                await _ecDataProcessingRepository.InsertOneAsync(body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECProductListResponse> GetProductList()
        {
            try
            {
                var timeStemp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

                var productListRequest = new ECProductListDto
                {
                    Channel = _ecConfig.Channel,
                    PartnerCode = _ecConfig.PartnerCode,
                    RequestId = _ecConfig.PartnerCode + timeStemp
                };

                var token = await _ecAuthorizationService.GetToken();
                var response = await _ecRestService.GetProductList(productListRequest, token);
                var result = response.ToObject<ECProductListResponse>();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECEligigleResponse> CheckEligigle(string customerId)
        {
            try
            {
                var customerDetail = await _customerCollection.FindOneAsync(x => !x.IsDeleted && x.Id == customerId);
                var timeStemp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

                ECRestEligibleDto request = new ECRestEligibleDto
                {
                    Channel = _ecConfig.Channel,
                    PartnerCode = _ecConfig.PartnerCode,
                    RequestId = _ecConfig.PartnerCode + timeStemp,
                    CustomerName = customerDetail?.Personal?.Name,
                    Email = customerDetail?.Personal?.Email,
                    DateOfBirth = customerDetail?.Personal?.DateOfBirth?.Replace("/", "-"),
                    IssueDate = customerDetail?.Personal?.IdCardDate?.Replace("/", "-"),
                    IdentityCardId = customerDetail?.Personal?.IdCard,
                    Profession = customerDetail?.Working?.JobId,
                    IssuePlace = customerDetail?.Personal?.IdCardProvinceId,
                    PhoneNumber = customerDetail?.Personal?.Phone,
                    TemProvince = customerDetail?.TemporaryAddress?.ProvinceId,
                    DsaAgentCode = _ecConfig.SaleCode,
                    SalesCode = customerDetail.SaleInfo?.EcSaleCode ?? _ecConfig.SaleCode
                };

                var token = await _ecAuthorizationService.GetToken();
                var result = await _ecRestService.CheckEligigle(request, token);
                var eligigleResponse = result.ToObject<ECEligigleResponse>();

                return eligigleResponse;

            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<EcErrorDto>();
                throw new ArgumentException(error?.Error?.ErrorMessage ?? ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECEligigleResponse> RemoteCheckingEligigle(ECEligibleDto request)
        {
            try
            {
                var timeStemp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

                ECRestEligibleDto restRequest = new ECRestEligibleDto
                {
                    RequestId = _ecConfig.PartnerCode + timeStemp,
                    Channel = _ecConfig.Channel,
                    PartnerCode = _ecConfig.PartnerCode,
                    SalesCode = _ecConfig.SaleCode,
                    CustomerName = request?.CustomerName,
                    Email = request?.Email,
                    DateOfBirth = request?.DateOfBirth,
                    IssueDate = request.IssueDate,
                    IdentityCardId = request?.IdentityCardId,
                    Profession = request?.Profession,
                    IssuePlace = request?.IssuePlace,
                    PhoneNumber = request?.PhoneNumber,
                    TemProvince = request?.TemProvince,
                    DsaAgentCode = _ecConfig.SaleCode,
                };

                var token = await _ecAuthorizationService.GetToken();
                var result = await _ecRestService.CheckEligigle(restRequest, token);
                var eligigleResponse = result.ToObject<ECEligigleResponse>();

                return eligigleResponse;

            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<EcErrorDto>();
                throw new ArgumentException(error?.Error?.ErrorMessage ?? ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECFullLoanResponse> SendFullLoanTest(string customerId)
        {
            try
            {
                var token = await _ecAuthorizationService.GetToken();
                var body = JsonConvert.DeserializeObject<ECFullLoanDto>(customerId);
                var result = await _ecRestService.SendFullLoan(body, token);

                var eligigleResponse = result.ToObject<ECFullLoanResponse>();
                return eligigleResponse;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw new ArgumentException(ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECFullLoanDto> GetFullLoanInfo(string customerId)
        {
            try
            {
                var customerDetail = await _customerCollection.FindOneAsync(x => !x.IsDeleted && x.Id == customerId);
                var timeStemp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

                ECFullLoanDto fullLoanRequest = null;

                Referee referee1 = null;
                Referee referee2 = null;

                if (customerDetail.Referees != null && customerDetail.Referees.Count() > 0)
                {
                    referee1 = customerDetail.Referees.ToArray()[0];
                }

                if (customerDetail.Referees != null && customerDetail.Referees.Count() > 1)
                {
                    referee2 = customerDetail.Referees.ToArray()[1];
                }

                fullLoanRequest = new ECFullLoanDto
                {
                    // config
                    Channel = _ecConfig.Channel,
                    PartnerCode = _ecConfig.PartnerCode,
                    RequestId = _ecConfig.PartnerCode + timeStemp,

                    DsaAgentCode = customerDetail?.SaleInfo?.EcDsaCode,
                    SalesCode = customerDetail?.SaleInfo?.EcSaleCode,

                    DateOfBirth = customerDetail?.Personal?.DateOfBirth?.Replace("/", "-"),
                    Email = customerDetail?.Personal?.Email,
                    IssueDate = customerDetail?.Personal?.IdCardDate?.Replace("/", "-"),
                    IdentityCardId = customerDetail?.Personal?.IdCard,
                    CustomerName = customerDetail?.Personal?.Name,
                    Gender = customerDetail?.Personal?.Gender,
                    PhoneNumber = customerDetail?.Personal?.Phone,
                    MarriedStatus = customerDetail?.Personal?.MaritalStatusId,
                    IssuePlace = customerDetail?.Personal?.IdCardProvinceId,

                    // loan
                    LoanTenor = customerDetail?.Loan?.TermId?.ToNumber(),
                    LoanPurpose = customerDetail?.Loan?.PurposeId,
                    LoanAmount = customerDetail?.Loan?.Amount.ToDecimal(),

                    // bank
                    BeneficiaryName = customerDetail?.DisbursementInformation?.BeneficiaryName,
                    BankAccount = customerDetail?.DisbursementInformation?.BankAccount,
                    BankBranchCode = customerDetail?.DisbursementInformation?.BankBranchCodeId,
                    BankCode = customerDetail?.DisbursementInformation?.BankCodeId,

                    // work 
                    WorkplaceAddress = customerDetail?.Working?.CompanyAddress?.GetFullAddress(),
                    WorkplaceDistrict = customerDetail?.Working?.CompanyAddress?.DistrictId,
                    WorkplaceName = customerDetail?.Working?.CompanyName,
                    WorkplaceWard = customerDetail?.Working?.CompanyAddress?.WardId,
                    WorkplacePhone = customerDetail?.Working?.CompanyPhone,
                    WorkplaceProvince = customerDetail?.Working?.CompanyAddress?.ProvinceId,
                    TaxId = customerDetail?.Working?.TaxCode,
                    JobTitle = customerDetail?.Working?.JobId,
                    JobType = customerDetail?.Working?.JobId,
                    Profession = defaultProfession,
                    ProductType = customerDetail?.Loan?.ProductId,
                    EmploymentContract = "IT",

                    // income
                    IncomeMethod = customerDetail?.Working?.IncomeMethod,
                    IncomeReceivingDate = customerDetail?.Working?.DueDay?.Replace("/", "-"),
                    OtherIncome = customerDetail?.Working?.OtherIncome,
                    MonthlyIncome = customerDetail?.Working?.Income?.ToDecimal(),
                    MonthlyExpense = customerDetail?.Working?.MonthlyExpenese,

                    // permanent address
                    PermanentAddress = customerDetail?.ResidentAddress?.GetFullAddress(),
                    PermanentDistrict = customerDetail?.ResidentAddress?.DistrictId,
                    PermanentProvince = customerDetail?.ResidentAddress?.ProvinceId,
                    PermanentWard = customerDetail?.ResidentAddress?.WardId,

                    // temporary address
                    TemAddress = customerDetail?.TemporaryAddress?.GetFullAddress(),
                    TemProvince = customerDetail?.TemporaryAddress?.ProvinceId,
                    TemDistrict = customerDetail?.TemporaryAddress?.DistrictId,
                    TemWard = customerDetail?.TemporaryAddress?.WardId,

                    // relation 1
                    Relation1 = referee1?.RelationshipId,
                    Relation1Name = referee1?.Name,
                    Relation1PhoneNumber = referee1?.Phone,

                    // relation 2
                    Relation2 = referee2?.RelationshipId,
                    Relation2Name = referee2?.Name,
                    Relation2PhoneNumber = referee2?.Phone,

                    EmployeeType = customerDetail?.Working?.EmploymentStatusId,
                    IdentityCardId2 = customerDetail?.Personal?.OldIdCard,
                    NumberOfDependents = customerDetail?.Personal?.NoOfDependent,
                    DisbursementMethod = customerDetail?.DisbursementInformation?.DisbursementMethodId,
                    MailingAddress = customerDetail?.ResidentAddress?.FullAddress,
                };

                return fullLoanRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public async Task<ECFullLoanResponse> SendFullLoan(string customerId)
        {
            ECDataProcessing eCDataProcessing = new ECDataProcessing(customerId)
            {
                Status = EcDataProcessingStatus.IN_PROGRESS
            };
            var ecdataProcessingDetail = await _ecDataProcessingRepository.GetECDataProcessingByCustomerId(customerId);

            try
            {
                ECFullLoanResponse eligigleResponse = null;

                var fullLoanRequest = JsonConvert.DeserializeObject<ECFullLoanDto>(ecdataProcessingDetail.PayLoad);

                var token = await _ecAuthorizationService.GetToken();
                var result = await _ecRestService.SendFullLoan(fullLoanRequest, token);

                await _ecDataProcessingRepository.UpdateResponse(ecdataProcessingDetail.Id, JsonConvert.SerializeObject(result), EcDataProcessingStatus.FINISHED);

                eligigleResponse = result.ToObject<ECFullLoanResponse>();

                var update = Builders<Customer>.Update
                    .Set(x => x.ModifiedDate, DateTime.Now)
                    .Set(x => x.ECRequest, fullLoanRequest.RequestId)
                    .Set(x => x.Status, CustomerStatus.PROCESSING);

                await _customerCollection.UpdateOneAsync(x => x.Id == customerId, update);

                return eligigleResponse;
            }
            catch (ApiException ex)
            {
                await _ecDataProcessingRepository.UpdateResponse(ecdataProcessingDetail.Id, ex.Content, EcDataProcessingStatus.ERROR);

                _logger.LogInformation("SendFullLoan 370");
                _logger.LogError(ex, ex.Content);
                throw new ArgumentException(ex.Content);
            }
            catch (Exception ex)
            {
                await _ecDataProcessingRepository.UpdateResponse(ecdataProcessingDetail.Id, ex.Message, EcDataProcessingStatus.ERROR);

                _logger.LogInformation("SendFullLoan 378");
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECSelectOfferResponse> SelectOffer(ECSelectOfferDto request)
        {
            try
            {
                var token = await _ecAuthorizationService.GetToken();
                var result = await _ecRestService.SelectOffer(request, token);
                var eligigleResponse = result.ToObject<ECSelectOfferResponse>();
                return eligigleResponse;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw new ArgumentException(ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECUpdateStatusResponse> UpdateStatus(ECUpdateStatusDto request)
        {
            try
            {
                var ecupdateStatusResponse = new ECUpdateStatusResponse()
                {
                    Body = new ECUpdateStatusDataResponse()
                };

                ECUpdateStatusValidation validator = new ECUpdateStatusValidation();
                ValidationResult result = validator.Validate(request);

                if (!result.IsValid)
                {
                    ecupdateStatusResponse.Body.Code = 2;
                    ecupdateStatusResponse.Body.Message = ECUpdateStatus.WrongInvalidParams;

                    return ecupdateStatusResponse;
                }

                ECOfferData offerData = _mapper.Map<ECOfferData>(request.Data);
                offerData.Code = request.Code;
                offerData.Message = request.Message;
                await _ecOfferDataRepository.Create(offerData);
                
                var customerRequest = await _customerCollection.FindOneAsync(x => x.ECRequest == request.Data.RequestId);

                if (customerRequest == null)
                {
                    ecupdateStatusResponse.Body.Code = 3;
                    ecupdateStatusResponse.Body.Message = ECUpdateStatus.LeadDoesNotExist;

                    return ecupdateStatusResponse;
                }

                var customerBefore = customerRequest.Clone();

                var customerResult = customerRequest.Result ?? new Result();
                customerResult.ReturnStatus = request.Code;

                string mappingStatus = EcDataMapping.STATUS.TryGetValue(request.Code, out string status) ? status : request.Code;
                customerResult.Reason = EcDataMapping.REJECT_REASON.TryGetValue($"{request.Code}-{request.Data.RejectReason}", out string reason) ? reason : request.Message;
                customerResult.ContractNumber = request.Data.ContractNumber;
                customerResult.RejectReason = request.Data.RejectReason;
                string contractCode = customerRequest.ContractCode; 
                if (string.IsNullOrEmpty(contractCode) && !string.IsNullOrEmpty(offerData.ProposalId)){
                    contractCode = "EC-" + offerData.ProposalId;
                }
                var update = Builders<Customer>.Update
                    .Set(x => x.ModifiedDate, DateTime.Now)
                    .Set(x => x.Status, mappingStatus)
                    .Set(x => x.ContractCode, contractCode)
                    .Set(x => x.Result, customerResult);
                await _customerCollection.UpdateOneAsync(x => x.Id == customerRequest.Id, update);

                _dataCRMProcessingServices.InsertOne(new Models.CRM.DataCRMProcessing
                {
                    CustomerId = customerRequest.Id,
                    LeadSource = LeadSourceType.EC
                });

                var customerAfterUdate = await _customerCollection.FindOneAsync(x => x.ECRequest == request.Data.RequestId);
                await _historyDomainService.CreateAsync(customerRequest.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStatus), customerBefore, customerAfterUdate);
                ecupdateStatusResponse.StatusCode = 200;
                ecupdateStatusResponse.Body.Message = ECUpdateStatus.Success;
                ecupdateStatusResponse.Body.Code = 0;

                return ecupdateStatusResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECUpdateStatusResponse> CreateNotification(ECNotification request)
        {
            try
            {
                var notification = _mapper.Map<ECNotification>(request);
                var rs = await _ecNotificationService.Create(notification);
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECSelectOfferResponse> SelectOfferAsync(string customerId)
        {
            ECDataProcessing eCDataProcessing = new ECDataProcessing(customerId)
            {
                Status = EcDataProcessingStatus.IN_PROGRESS
            };

            try
            {
                var customer = await _customerCollection.FindOneAsync(x => !x.IsDeleted && x.Id == customerId);
                ECSelectOfferDto eCSelectOfferDto = new ECSelectOfferDto
                {
                    LoanRequestId = customer.ECRequest,
                    PartnerCode = _ecConfig.PartnerCode,
                    SelectedOfferId = customer.LeadEcSelectedOffer?.SelectedOfferId,
                    SelectedOfferAmount = customer.LeadEcSelectedOffer?.SelectedOfferAmount,
                    SelectedOfferInsuranceType = customer.LeadEcSelectedOffer?.SelectedOfferInsuranceType
                };
                eCDataProcessing.PayLoad = JsonConvert.SerializeObject(eCSelectOfferDto);

                var token = await _ecAuthorizationService.GetToken();
                var result = await _ecRestService.SelectOffer(eCSelectOfferDto, token);
                eCDataProcessing.Response = JsonConvert.SerializeObject(result);
                eCDataProcessing.Status = EcDataProcessingStatus.FINISHED;
                await _ecDataProcessingRepository.InsertOneAsync(eCDataProcessing);

                var eligigleResponse = result.ToObject<ECSelectOfferResponse>();

                return eligigleResponse;
            }
            catch (ApiException ex)
            {
                eCDataProcessing.Response = JsonConvert.SerializeObject(ex);
                eCDataProcessing.Message = ex.Content;
                eCDataProcessing.Status = EcDataProcessingStatus.ERROR;
                await _ecDataProcessingRepository.InsertOneAsync(eCDataProcessing);

                _logger.LogError(ex, ex.Content);
                throw new ArgumentException(ex.Content);
            }
            catch (Exception ex)
            {
                eCDataProcessing.Response = JsonConvert.SerializeObject(ex);
                eCDataProcessing.Message = ex.Message;
                eCDataProcessing.Status = EcDataProcessingStatus.ERROR;
                await _ecDataProcessingRepository.InsertOneAsync(eCDataProcessing);

                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ECDataProsessingDetailResponse>> GetDataProsessingByCustomerId(string customerId)
        {
            try
            {
                var result = await _ecDataProcessingRepository.GetListByCustomerId(customerId);
                var response = _mapper.Map<IEnumerable<ECDataProsessingDetailResponse>>(result);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}