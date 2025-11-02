using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.PtfOmnis
{
    public interface IPtfOmniService
    {
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniMasterDataVersionResponse>>> GetMasterDataVersionAsync(object body);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniMasterDataListResponse>>> GetMasterDataListAsync(PtfOmniMasterDataListRequest body);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCifResponse>>> GetFetchServiceCifAsync(PtfOmniFetchServiceCifRequest body);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCbsResponse>>> GetFetchServiceCbsAsync(PtfOmniFetchServiceCbsRequest body);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceLosResponse>>> GetFetchServiceLosAsync(PtfOmniFetchServiceLosRequest body);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceBlackListResponse>>> GetFetchServiceBlackListAsync(PtfOmniFetchServiceBlackListRequest body);

        Task<PtfOmniResponseModel<object>> DeleteDocumentsAsync(PtfOmniDocumentDeleteRequest body);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniDocumentListResponse>>> GetDocumentListAsync(PtfOmniDocumentListRequest body);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniDocumentUrlResponse>>> GetDocumentUrlAsync(PtfOmniDocumentUrlRequest body);

        Task<PtfOmniResponseModel<PtfOmniDocumentUploadResponse>> UploadDocumentAsync(PtfOmniDocumentUploadRequest body);

        Task<PtfOmniResponseModel<PtfOmniLoanNewCaseResponse>> GetLoanNewCaseAsync();

        Task<PtfOmniResponseModel<object>> CreateLoanAsync(PtfOmniLoanCreateRequest body);

        Task<PtfOmniResponseModel<PtfOmniLoanDetailResponse>> GetLoanDetailAsync(string caseId);

        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniLoanListResponse>>> GetLoanListAsync(PtfOmniLoanListRequest body);

        Task<PtfOmniResponseModel<object>> UpdateLoanAsync(PtfOmniLoanUpdateRequest body);

        Task<PtfOmniResponseModel<object>> SaveDecisionLoanAsync(PtfOmniLoanSaveDecisionRequest body);

        Task<PtfOmniFetchServiceResponse> CheckValidLoanAsync(string customerId);

        Task<PtfOmniFetchServiceResponse> CheckValidLoanAsync(PtfOmniCheckValidLoanRequest request);

        Task UpdateCustomerStatusAsync();

        Task SyncLoanAsync(string customerId);

        Task CancelLoanApplicationAsync(string customerId, PtfOmniCancelApplicationRequest ptfOmniCancelApplicationRequest);
    }

    public class PtfOmniService : IPtfOmniService, IScopedLifetime
    {
        private readonly ILogger<PtfOmniService> _logger;
        private readonly PtfOmniConfig _ptfOmniConfig;
        private readonly IRestPtfOmniService _restPtfOmniService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly IUserLoginService _userLoginService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;

        public PtfOmniService(
            ILogger<PtfOmniService> logger,
            IOptions<PtfOmniConfig> ptfOmniConfig,
            IRestPtfOmniService restPtfOmniService,
            ICustomerRepository customerRepository,
            IHistoryDomainService historyDomainService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IUserLoginService userLoginService)
        {
            _logger = logger;
            _ptfOmniConfig = ptfOmniConfig.Value;
            _restPtfOmniService = restPtfOmniService;
            _customerRepository = customerRepository;
            _historyDomainService = historyDomainService;
            _userLoginService = userLoginService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
        }

        public Task<PtfOmniResponseModel<object>> CreateLoanAsync(PtfOmniLoanCreateRequest body)
        {
            try
            {
                return _restPtfOmniService.CreateLoanAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<object>> DeleteDocumentsAsync(PtfOmniDocumentDeleteRequest body)
        {
            try
            {
                return _restPtfOmniService.DeleteDocumentsAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniDocumentListResponse>>> GetDocumentListAsync(PtfOmniDocumentListRequest body)
        {
            try
            {
                return _restPtfOmniService.GetDocumentListAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniDocumentUrlResponse>>> GetDocumentUrlAsync(PtfOmniDocumentUrlRequest body)
        {
            try
            {
                return _restPtfOmniService.GetDocumentUrlAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceBlackListResponse>>> GetFetchServiceBlackListAsync(PtfOmniFetchServiceBlackListRequest body)
        {
            try
            {
                return _restPtfOmniService.GetFetchServiceBlackListAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCbsResponse>>> GetFetchServiceCbsAsync(PtfOmniFetchServiceCbsRequest body)
        {
            try
            {
                return _restPtfOmniService.GetFetchServiceCbsAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCifResponse>>> GetFetchServiceCifAsync(PtfOmniFetchServiceCifRequest body)
        {
            try
            {
                return _restPtfOmniService.GetFetchServiceCifAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceLosResponse>>> GetFetchServiceLosAsync(PtfOmniFetchServiceLosRequest body)
        {
            try
            {
                return _restPtfOmniService.GetFetchServiceLosAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PtfOmniResponseModel<PtfOmniLoanDetailResponse>> GetLoanDetailAsync(string caseId)
        {
            try
            {

                var customer = await _customerRepository.FindByIdAsync(caseId);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                return await _restPtfOmniService.GetLoanDetailAsync(clientId, secretKey, caseId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniLoanListResponse>>> GetLoanListAsync(PtfOmniLoanListRequest body)
        {
            try
            {
                return _restPtfOmniService.GetLoanListAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PtfOmniResponseModel<PtfOmniDocumentUploadResponse>> UploadDocumentAsync(PtfOmniDocumentUploadRequest body)
        {
            try
            {
                var client = new RestClient($"{_ptfOmniConfig.Host}/documents/upload-multipart");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-CLIENT-ID", _ptfOmniConfig.ClientId);
                request.AddHeader("X-SECRET-API", _ptfOmniConfig.SecretId);
                request.AddParameter("caseId", body.CaseId);
                request.AddParameter("documentCategory", body.DocumentCategory);
                request.AddParameter("documentType", body.DocumentType);
                if (body.File != null)
                {
                    using var ms = new MemoryStream();
                    body.File.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    request.AddFile("file", fileBytes, body.File.FileName, body.File.ContentType);

                }
                IRestResponse response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<PtfOmniResponseModel<PtfOmniDocumentUploadResponse>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniLoanNewCaseResponse>> GetLoanNewCaseAsync()
        {
            try
            {
                return _restPtfOmniService.GetLoanNewCaseAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniMasterDataListResponse>>> GetMasterDataListAsync(PtfOmniMasterDataListRequest body)
        {
            try
            {
                return _restPtfOmniService.GetMasterDataListAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniMasterDataVersionResponse>>> GetMasterDataVersionAsync(object body)
        {
            try
            {
                return _restPtfOmniService.GetMasterDataVersionAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<object>> SaveDecisionLoanAsync(PtfOmniLoanSaveDecisionRequest body)
        {
            try
            {
                return _restPtfOmniService.SaveDecisionLoanAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<PtfOmniResponseModel<object>> UpdateLoanAsync(PtfOmniLoanUpdateRequest body)
        {
            try
            {
                return _restPtfOmniService.UpdateLoanAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PtfOmniFetchServiceResponse> CheckValidLoanAsync(string customerId)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(customerId);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                var cifCbsTask = await Task.Factory.StartNew(async () =>
                {
                    var cif = await GetFetchServiceCifAsync(clientId, secretKey, new List<string> { customer.Personal.IdCard, customer.Personal.OldIdCard });
                    var cbs = await GetFetchServiceCbsAsync(clientId, secretKey, cif?.Data?.Items?.Select(x => x.Cif) ?? new List<string>());
                    return new
                    {
                        cif,
                        cbs
                    };
                });
                var losTask = GetFetchServiceLosAsync(clientId, secretKey, customer.FamilyBookNo,
                    new List<string> { customer.Personal.IdCard, customer.Personal.OldIdCard },
                    new List<string> { customer.Personal.Phone, customer.Personal.OldPhone, customer.Personal.SubPhone });
                var blackListTask = GetFetchServiceBlackListAsync(clientId, secretKey, customer.FamilyBookNo,
                    new List<string> { customer.Personal.IdCard, customer.Personal.OldIdCard },
                    new List<string> { customer.Personal.Phone, customer.Personal.OldPhone, customer.Personal.SubPhone });

                await Task.WhenAll(cifCbsTask, losTask, blackListTask);

                return new PtfOmniFetchServiceResponse
                {
                    FetchServiceCif = cifCbsTask.Result.cif,
                    FetchServiceCbs = cifCbsTask.Result.cbs,
                    FetchServiceLos = losTask.Result,
                    FetchServiceBlackList = blackListTask.Result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PtfOmniFetchServiceResponse> CheckValidLoanAsync(PtfOmniCheckValidLoanRequest request)
        {
            try
            {
                var cifCbsTask = await Task.Factory.StartNew(async () =>
                {
                    var cif = await GetFetchServiceCifAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, request.IdCards);
                    var cbs = await GetFetchServiceCbsAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, cif?.Data?.Items?.Select(x => x.Cif) ?? new List<string>());
                    return new
                    {
                        cif,
                        cbs
                    };
                });
                var losTask = GetFetchServiceLosAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, request.FamilyBookNo, request.IdCards, request.Phones);
                var blackListTask = GetFetchServiceBlackListAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, request.FamilyBookNo, request.IdCards, request.Phones);

                await Task.WhenAll(cifCbsTask, losTask, blackListTask);

                return new PtfOmniFetchServiceResponse
                {
                    FetchServiceCif = cifCbsTask.Result.cif,
                    FetchServiceCbs = cifCbsTask.Result.cbs,
                    FetchServiceLos = losTask.Result,
                    FetchServiceBlackList = blackListTask.Result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateCustomerStatusAsync()
        {
            try
            {
                var customers = (await _customerRepository.FilterByAsync(x =>
                    x.GreenType == GreenType.GreenP &&
                    x.Status == CustomerStatus.PROCESSING &&
                    !string.IsNullOrEmpty(x.PtfCaseId) &&
                    x.TimeToRunJob < DateTime.Now)
                ).ToList();

                if (!customers.Any())
                {
                    return;
                }

                foreach (var customer in customers)
                {
                    await SyncLoanAsync(customer.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SyncLoanAsync(string customerId)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(customerId);
                if (customer == null)
                {
                    throw new ArgumentException(Message.COMMON_NOT_FOUND, nameof(Customer));
                }

                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                var loan = await _restPtfOmniService.GetLoanDetailAsync(clientId, secretKey, customer.PtfCaseId);
                var valueBefore = customer.Clone();

                if (!loan.Success)
                {
                    throw new ArgumentException(loan.Error.Message);
                }

                switch (loan.Data.QueueName)
                {
                    case "PTF_Disbursement_Checker":
                        customer.Status = CustomerStatus.SUCCESS;
                        break;
                    case "PTF_Cancel_Permanent":
                    case "PTF_Delete":
                        customer.Status = CustomerStatus.CANCEL;
                        break;
                    case "PTF_Reject_Permanent":
                    case "PTF_Reject_Revoke":
                        customer.Status = CustomerStatus.REJECT;
                        break;
                    case "PTF_Request_Initiate":
                    case "PTF_Data_Entry":
                    case "PTF_Document_Check":
                    case "PTF_Telephone_Verify":
                    case "PTF_Field_Verify":
                    case "PTF_Credit_Approval":
                    case "PTF_Predisbursement":
                    case "PTF_KYC_Maker":
                    case "PTF_KYC_Checker":
                    case "PTF_Disbursement_Info_Maker":
                    case "PTF_Disbursement_Info_Checker":
                    case "PTF_Disbursement_Maker":
                    case "PTF_Anti_Fraud":
                        customer.Status = CustomerStatus.PROCESSING;
                        break;
                }

                if (loan.Data.Status == (int)PtfOmniLoanStatus.Return
                    || (!string.IsNullOrEmpty(loan.Data.PreQueue) && loan.Data.QueueName == "PTF_Data_Entry"))
                {
                    customer.Status = CustomerStatus.RETURN;
                }

                customer.Result.ReturnStatus = loan.Data.QueueName;
                customer.Result.Status = loan.Data.PreQueue;
                customer.Result.Note = loan.Data.PreRemark;
                customer.Result.Reason = string.Join(", ", new[] { loan.Data.LeaderNote, loan.Data.CurrentRemark }.Where(x => !string.IsNullOrEmpty(x)));
                // Increase time to run job
                var timeToRunJob = DateTime.Now.AddHours(1);
                // If now bigger than 21:00 PM, add more 12h => 09:00 next day to run job
                // 14 =21 -7 (GTM)
                if (DateTime.Now.Hour > 14)
                {
                    timeToRunJob.AddHours(12);
                }

                var update = Builders<Customer>.Update
                    .Set(x => x.Status, customer.Status)
                    .Set(x => x.Result, customer.Result)
                    .Set(x => x.Modifier, _userLoginService.GetUserId())
                    .Set(x => x.TimeToRunJob, timeToRunJob)
                    .Set(x => x.ModifiedDate, DateTime.Now);

                await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);
                if (valueBefore.Result.ReturnStatus != customer.Result.ReturnStatus ||
                    valueBefore.Result.Status != customer.Result.Status ||
                    valueBefore.Status != customer.Status)
                {
                    _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                    {
                        CustomerId = customer.Id,
                        LeadSource = LeadSourceType.PTF,
                    });

                    await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SyncLoanAsync), valueBefore, customer);
                }
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task CancelLoanApplicationAsync(string customerId, PtfOmniCancelApplicationRequest ptfOmniCancelApplicationRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(customerId);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                var loan = await _restPtfOmniService.GetLoanDetailAsync(clientId, secretKey, customer.PtfCaseId);

                if (!loan.Success)
                {
                    throw new ArgumentException(loan.Error.Message);
                }

                if (loan.Data?.QueueName != "PTF_Data_Entry")
                {
                    throw new ArgumentException(string.Format(Message.PTF_CANCEL_LOAN_APPLICATION_WRONG_STATUS, loan.Data.QueueName));
                }

                var request = new PtfOmniLoanSaveDecisionRequest
                {
                    CaseId = customer.PtfCaseId,
                    DecisionId = $"{(int)PtfOmniSaveDicesion.Cancel}",
                    DeRemark = ptfOmniCancelApplicationRequest.Reason
                };

                var response = await _restPtfOmniService.SaveDecisionLoanAsync(clientId, secretKey, request);
                if (!response.Success)
                {
                    throw new ArgumentException(response.Error.Message);
                }

                var valueBefore = customer.Clone();

                customer.Status = CustomerStatus.CANCEL;
                customer.Result.Reason = ptfOmniCancelApplicationRequest.Reason;

                var update = Builders<Customer>.Update
                    .Set(x => x.Status, customer.Status)
                    .Set(x => x.Modifier, _userLoginService.GetUserId())
                    .Set(x => x.ModifiedDate, DateTime.Now);

                await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    LeadSource = LeadSourceType.PTF,
                });
                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(CancelLoanApplicationAsync), valueBefore, customer);
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCifResponse>>> GetFetchServiceCifAsync(string clientId, string secretKey, IEnumerable<string> idCards)
        {
            try
            {
                return await _restPtfOmniService.GetFetchServiceCifAsync(clientId, secretKey, new PtfOmniFetchServiceCifRequest
                {
                    IdDocumentNo = idCards
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCifResponse>>
                {
                    Error = new PtfOmniResponseErrorModel
                    {
                        Errors = new List<object>
                        {
                            new
                            {
                                Error = ex.Message
                            }
                        }
                    }
                };
            }
        }

        private async Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCbsResponse>>> GetFetchServiceCbsAsync(string clientId, string secretKey, IEnumerable<string> cifs)
        {
            try
            {
                return await _restPtfOmniService.GetFetchServiceCbsAsync(clientId, secretKey, new PtfOmniFetchServiceCbsRequest { Cif = cifs });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCbsResponse>>
                {
                    Error = new PtfOmniResponseErrorModel
                    {
                        Errors = new List<object>
                        {
                            new
                            {
                                Error = ex.Message
                            }
                        }
                    }
                };
            }
        }

        private async Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceLosResponse>>> GetFetchServiceLosAsync(string clientId, string secretKey, string familyBookNo,
            IEnumerable<string> idCards, IEnumerable<string> phones)
        {
            try
            {
                return await _restPtfOmniService.GetFetchServiceLosAsync(clientId, secretKey, new PtfOmniFetchServiceLosRequest
                {
                    FrbDocumentNo = !string.IsNullOrEmpty(familyBookNo) ? familyBookNo : null,
                    IdDocumentNo = idCards != null && idCards.Count() > 0 && !string.IsNullOrEmpty(idCards.ToList().First()) ? idCards : null,
                    PhoneNumber = phones != null && phones.Count() > 0 && !string.IsNullOrEmpty(phones.ToList().First()) ? phones : null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceLosResponse>>
                {
                    Error = new PtfOmniResponseErrorModel
                    {
                        Errors = new List<object>
                        {
                            new
                            {
                                Error = ex.Message
                            }
                        }
                    }
                };
            }
        }

        private async Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceBlackListResponse>>> GetFetchServiceBlackListAsync(string clientId, string secretKey, string familyBookNo,
            IEnumerable<string> idCards, IEnumerable<string> phones)
        {
            try
            {
                return await _restPtfOmniService.GetFetchServiceBlackListAsync(clientId, secretKey, new PtfOmniFetchServiceBlackListRequest
                {
                    FrbDocumentNo = !string.IsNullOrEmpty(familyBookNo) ? familyBookNo : null,
                    IdDocumentNo = idCards != null && idCards.Count() > 0 && !string.IsNullOrEmpty(idCards.ToList().First()) ? idCards : null,
                    PhoneNumber = phones != null && phones.Count() > 0 && !string.IsNullOrEmpty(phones.ToList().First()) ? phones : null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceBlackListResponse>>
                {
                    Error = new PtfOmniResponseErrorModel
                    {
                        Errors = new List<object>
                        {
                            new
                            {
                                Error = ex.Message
                            }
                        }
                    }
                };
            }
        }

        private (string, string) GetAccountSetting(SaleChanelInfo saleChanelInfo)
        {
            var clientId = string.IsNullOrEmpty(saleChanelInfo?.SaleChanelConfigUserInfo?.ClientID) ? _ptfOmniConfig.ClientId : saleChanelInfo.SaleChanelConfigUserInfo.ClientID;
            var secretKey = string.IsNullOrEmpty(saleChanelInfo?.SaleChanelConfigUserInfo?.SecretKey) ? _ptfOmniConfig.SecretId : saleChanelInfo.SaleChanelConfigUserInfo.SecretKey;
            return (clientId, secretKey);
        }
    }
}
