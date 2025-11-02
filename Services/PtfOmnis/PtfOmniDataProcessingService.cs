using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.PtfOmnis;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Services.Storage;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
    public interface IPtfOmniDataProcessingService
    {
        Task CreateOneAsync(Customer customer);
        Task<PagingResponse<PtfOmniGetDataProcessingResponse>> GetAsync(PtfOmniGetDataProcessingRequest ptfOmniGetDataProcessingRequest);
        Task UpdateAsync(string id, PtfOmniUpdateDataProcessingRequest dto);
        Task SyncDataAsync();
        Task SyncDataAsync(string customerId);

    }
    public class PtfOmniDataProcessingService : IPtfOmniDataProcessingService, IScopedLifetime
    {
        private readonly ILogger<PtfOmniDataProcessingService> _logger;
        private readonly IMapper _mapper;
        private readonly IPtmOmniDataProcessingRepository _ptmOmniDataProcessingRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRestPtfOmniService _restPtfOmniService;
        private readonly PtfOmniConfig _ptfOmniConfig;
        private readonly IStorageService _storageService;
        private readonly IPtfOmniMasterDataRepository _ptfOmniMasterDataRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<ChecklistModel> _checklistModelRepository;

        public PtfOmniDataProcessingService(
            ILogger<PtfOmniDataProcessingService> logger,
            IMapper mapper,
            IPtmOmniDataProcessingRepository ptmOmniDataProcessingRepository,
            ICustomerRepository customerRepository,
            IRestPtfOmniService restPtfOmniService,
            IOptions<PtfOmniConfig> ptfOmniConfig,
            IMongoRepository<ChecklistModel> checklistModelRepository,
            IStorageService storageService,
            IPtfOmniMasterDataRepository ptfOmniMasterDataRepository,
            DataCRMProcessingServices dataCRMProcessingServices,
            IHistoryDomainService historyDomainService)
        {
            _logger = logger;
            _mapper = mapper;
            _ptmOmniDataProcessingRepository = ptmOmniDataProcessingRepository;
            _customerRepository = customerRepository;
            _restPtfOmniService = restPtfOmniService;
            _ptfOmniConfig = ptfOmniConfig.Value;
            _storageService = storageService;
            _ptfOmniMasterDataRepository = ptfOmniMasterDataRepository;
            _historyDomainService = historyDomainService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _checklistModelRepository = checklistModelRepository;
        }

        public async Task CreateOneAsync(Customer customer)
        {
            try
            {
                var ptmOmniDataProcessing = await _ptmOmniDataProcessingRepository.FindOneAsync(x => x.Status == PtfOmniDataProcessingStatus.Draft && x.CustomerId == customer.Id);
                if (ptmOmniDataProcessing != null)
                {
                    return;
                }

                ptmOmniDataProcessing = await _ptmOmniDataProcessingRepository.FindOneAsync(x => x.Status == PtfOmniDataProcessingStatus.Error && x.CustomerId == customer.Id);
                if (ptmOmniDataProcessing != null)
                {
                    await UpdatePtfOmniDataProcessingStatusAsync(ptmOmniDataProcessing, PtfOmniDataProcessingStatus.Draft);
                    return;
                }

                await _ptmOmniDataProcessingRepository.InsertOneAsync(new PtfOmniDataProcessing
                {
                    CustomerId = customer.Id,
                    Step = string.IsNullOrEmpty(customer.ContractCode) ? DataProcessingStep.PTF_CREATE_LOAN : DataProcessingStep.PTF_UPDATE_LOAN
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task UpdateAsync(string id, PtfOmniUpdateDataProcessingRequest dto)
        {
            try
            {
                var ptmOmniDataProcessing = await _ptmOmniDataProcessingRepository.FindOneAsync(x => x.Id == id);
                if (ptmOmniDataProcessing != null)
                {
                    var update = Builders<PtfOmniDataProcessing>.Update
                        .Set(x => x.Status, dto.Status)
                        .Set(x => x.Step, dto.Step)
                        .Set(x => x.ModifiedDate, DateTime.Now);
                    await _ptmOmniDataProcessingRepository.UpdateOneAsync(x => x.Id == id, update);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<PagingResponse<PtfOmniGetDataProcessingResponse>> GetAsync(PtfOmniGetDataProcessingRequest ptfOmniGetDataProcessingRequest)
        {
            try
            {
                var ptmOmniDataProcessings = await _ptmOmniDataProcessingRepository.GetAsync(
                    ptfOmniGetDataProcessingRequest.CustomerId,
                    ptfOmniGetDataProcessingRequest.PageIndex,
                    ptfOmniGetDataProcessingRequest.PageSize);

                var total = await _ptmOmniDataProcessingRepository.CountAsync(ptfOmniGetDataProcessingRequest.CustomerId);

                var result = new PagingResponse<PtfOmniGetDataProcessingResponse>
                {
                    TotalRecord = total,
                    Data = ptmOmniDataProcessings
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SyncDataAsync()
        {
            try
            {
                var ptfOmniDataProcessings = await _ptmOmniDataProcessingRepository.FilterByAsync(
                    x => x.Status == PtfOmniDataProcessingStatus.Draft,
                    _ptfOmniConfig.NumberOfRecordPerProcess);

                if (!ptfOmniDataProcessings.Any())
                {
                    return;
                }

                var customerIds = ptfOmniDataProcessings.Select(x => x.CustomerId);
                var customers = await _customerRepository.FilterByAsync(x => customerIds.Contains(x.Id));

                foreach (var customer in customers)
                {
                    var ptfOmniDataProcessing = ptfOmniDataProcessings
                        .FirstOrDefault(x => x.CustomerId == customer.Id);
                    await SyncDataAsync(customer, ptfOmniDataProcessing);
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

        public async Task SyncDataAsync(string customerId)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(customerId);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }
                var process = await _ptmOmniDataProcessingRepository.FindOneAsync(x => x.CustomerId == customerId && x.Status == PtfOmniDataProcessingStatus.Draft);

                await SyncDataAsync(customer, process);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }


        private async Task SyncDataAsync(Customer customer, PtfOmniDataProcessing ptfOmniDataProcessing)
        {
            try
            {
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing, PtfOmniDataProcessingStatus.InProgress);

                //PtfOmniResponseModel<object> response = null;

                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);

                var valueBefore = customer.Clone();

                switch (ptfOmniDataProcessing.Step)
                {
                    case DataProcessingStep.PTF_CREATE_LOAN:
                        {
                            if (string.IsNullOrEmpty(customer.PtfCaseId))
                            {
                                var newCase = await _restPtfOmniService.GetLoanNewCaseAsync(clientId, secretKey);
                                if (newCase.Success)
                                {
                                    customer.PtfCaseId = newCase.Data.CaseId;

                                    var updatePtfCaseId = Builders<Customer>.Update
                                        .Set(x => x.PtfCaseId, customer.PtfCaseId);

                                    await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, updatePtfCaseId);
                                }
                                else
                                {
                                    customer.Status = CustomerStatus.RETURN;
                                    customer.Result.Reason = newCase.GetErrorMessage();
                                    ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Error;
                                    break;
                                }
                            }
                            var response = await CreateLoanAsync(customer, ptfOmniDataProcessing);
                            if (response.Success)
                            {
                                var updateCreate = Builders<Customer>.Update
                                    .Set(x => x.ContractCode, customer.PtfCaseId);

                                await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, updateCreate);
                                await UpdateStepAsync(ptfOmniDataProcessing.Id, DataProcessingStep.PTF_UPLOAD_DOCUMENT);
                                goto case DataProcessingStep.PTF_UPLOAD_DOCUMENT;
                            }
                            else
                            {
                                customer.Status = CustomerStatus.RETURN;
                                customer.Result.Reason = response.GetErrorMessage();
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Error;
                            }

                            break;
                        }
                    case DataProcessingStep.PTF_UPLOAD_DOCUMENT:
                        {
                            var response = await UploadDocumentAsync(customer, ptfOmniDataProcessing);
                            if (response.Success)
                            {
                                customer.Status = CustomerStatus.PROCESSING;
                                customer.Result.Reason = string.Empty;
                                customer.Result.Note = string.Empty;
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Done;
                                await UpdateStepAsync(ptfOmniDataProcessing.Id, DataProcessingStep.PTF_DONE);
                            }
                            else
                            {
                                customer.Status = CustomerStatus.RETURN;
                                if (response.Error == null)
                                {
                                    if (customer.Documents.Any(x => string.IsNullOrEmpty(x.GroupCode)))
                                    {
                                        var checkList = await _checklistModelRepository.FindOneAsync(x =>
                                            x.GreenType == GreenType.GreenP && x.ProductLine == customer.ProductLine);
                                        customer.Documents = checkList?.Checklist;
                                    }
                                    customer.Result.Reason = "Cần upload lại bộ chứng từ";
                                }
                                else
                                {
                                    customer.Result.Reason = response.GetErrorMessage();
                                }
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Error;
                            }
                            var updateDocuments = Builders<Customer>.Update
                                .Set(x => x.Documents, customer.Documents);

                            await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, updateDocuments);
                            break;
                        }
                    case DataProcessingStep.PTF_UPDATE_LOAN:
                        {
                            var response = await UpdateLoanAsync(customer, ptfOmniDataProcessing);
                            if (response.Success)
                            {
                                await UpdateStepAsync(ptfOmniDataProcessing.Id, DataProcessingStep.PTF_REUPLOAD_DOCUMENT);
                                goto case DataProcessingStep.PTF_REUPLOAD_DOCUMENT;
                            }
                            else
                            {
                                customer.Status = CustomerStatus.RETURN;
                                customer.Result.Reason = response.GetErrorMessage();
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Error;
                            }
                            break;
                        }
                    case DataProcessingStep.PTF_CHANGE_STATUS:
                        {
                            var response = await ChangeStatusAsync(customer, ptfOmniDataProcessing);
                            if (response.Success)
                            {
                                customer.Status = CustomerStatus.PROCESSING;
                                customer.Result.Reason = string.Empty;
                                customer.Result.Note = string.Empty;
                                await UpdateStepAsync(ptfOmniDataProcessing.Id, DataProcessingStep.PTF_DONE);
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Done;
                            }
                            else
                            {
                                customer.Status = CustomerStatus.RETURN;
                                customer.Result.Reason = response.GetErrorMessage();
                                customer.Status = CustomerStatus.RETURN;
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Error;
                            }
                            break;
                        }
                    case DataProcessingStep.PTF_REUPLOAD_DOCUMENT:
                        {
                            var response = await UploadDocumentAsync(customer, ptfOmniDataProcessing);
                            if (response.Success)
                            {
                                if (customer.Result.ReturnStatus == "PTF_Data_Entry" && !string.IsNullOrEmpty(customer.Result.Status))
                                {
                                    await UpdateStepAsync(ptfOmniDataProcessing.Id, DataProcessingStep.PTF_SAVE_DECISION);
                                    goto case DataProcessingStep.PTF_SAVE_DECISION;
                                }
                                else
                                {
                                    await UpdateStepAsync(ptfOmniDataProcessing.Id, DataProcessingStep.PTF_CHANGE_STATUS);
                                    goto case DataProcessingStep.PTF_CHANGE_STATUS;
                                }
                            }
                            else
                            {
                                customer.Status = CustomerStatus.RETURN;
                                customer.Result.Reason = response.GetErrorMessage();
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Error;
                            }
                            break;
                        }
                    case DataProcessingStep.PTF_SAVE_DECISION:
                        {
                            var response = await SaveDecisionAsync(customer, ptfOmniDataProcessing);
                            if (response.Success)
                            {
                                customer.Status = CustomerStatus.PROCESSING;
                                customer.Result.Reason = string.Empty;
                                customer.Result.Note = string.Empty;
                                await UpdateStepAsync(ptfOmniDataProcessing.Id, DataProcessingStep.PTF_DONE);
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Done;
                            }
                            else
                            {
                                customer.Status = CustomerStatus.RETURN;
                                customer.Result.Reason = response.GetErrorMessage();
                                ptfOmniDataProcessing.Status = PtfOmniDataProcessingStatus.Error;
                            }
                            break;
                        }
                    default:
                        break;
                }

                var update = Builders<Customer>.Update
                    .Set(x => x.Status, customer.Status)
                    .Set(x => x.Result, customer.Result)
                    .Set(x => x.TimeToRunJob, DateTime.Now.AddMinutes(15))
                    .Set(x => x.ModifiedDate, DateTime.Now);

                await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);

                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing, ptfOmniDataProcessing.Status);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SyncDataAsync), valueBefore, customer);
                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    LeadSource = LeadSourceType.PTF,
                });
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    message: ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    message: ex.Message);
            }
        }

        private async Task<PtfOmniResponseModel<object>> ChangeStatusAsync(Customer customer, PtfOmniDataProcessing ptfOmniDataProcessing)
        {
            PtfOmniLoanChangeStatusRequest request = null;
            try
            {
                request = new PtfOmniLoanChangeStatusRequest
                {
                    CaseId = customer.PtfCaseId,
                    Status = $"{(int)PtfOmniChangeStatus.Submit}",
                    Note = customer.CaseNote
                };
                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                var response = await _restPtfOmniService.ChangeStatusLoanAsync(clientId, secretKey, request);

                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    response.Success ? PtfOmniDataProcessingStatus.Done : PtfOmniDataProcessingStatus.Error,
                    request,
                    response,
                    response?.Error?.Message);

                return response;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Content);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Content } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Message);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Message } };
            }
        }

        private async Task<PtfOmniResponseModel<object>> SaveDecisionAsync(Customer customer, PtfOmniDataProcessing ptfOmniDataProcessing)
        {
            PtfOmniLoanSaveDecisionRequest request = null;
            try
            {
                request = new PtfOmniLoanSaveDecisionRequest
                {
                    CaseId = customer.PtfCaseId,
                    DecisionId = $"{(int)PtfOmniSaveDicesion.SubmitOrReSubmit}"
                };
                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                var response = await _restPtfOmniService.SaveDecisionLoanAsync(clientId, secretKey, request);

                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    response.Success ? PtfOmniDataProcessingStatus.Done : PtfOmniDataProcessingStatus.Error,
                    request,
                    response,
                    response?.Error?.Message);

                return response;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Content);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Content } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Message);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Message } };
            }
        }

        private async Task<PtfOmniResponseModel<object>> UpdateLoanAsync(Customer customer, PtfOmniDataProcessing ptfOmniDataProcessing)
        {
            PtfOmniLoanUpdateRequest request = null;
            try
            {
                request = _mapper.Map<PtfOmniLoanUpdateRequest>(customer);
                request.CustomerInfo.IdsCustomer = GetIdsCustomer(customer);
                request.SaleId = customer.ProductLine == "TSA" ? _ptfOmniConfig.SaleIdTSA : _ptfOmniConfig.SaleIdDSA;
                var quickProcesses = await _ptfOmniMasterDataRepository.FindMetadataAsync("PTF_LOS_MAS_QUICK_PROCESS", "credit_product_id", Int16.Parse(customer.Loan.ProductId));
                if (quickProcesses != null)
                {
                    request.RequestInfo.QuickProcessEnable = !string.IsNullOrEmpty(quickProcesses.Name) ? quickProcesses.Name : "No";
                }
                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                var response = await _restPtfOmniService.UpdateLoanAsync(clientId, secretKey, request);

                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    response.Success ? PtfOmniDataProcessingStatus.Done : PtfOmniDataProcessingStatus.Error,
                    request,
                    response,
                    response?.Error?.Message);

                return response;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Content);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Content } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Message);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Message } };
            }
        }

        private async Task<PtfOmniResponseModel<object>> CreateLoanAsync(Customer customer, PtfOmniDataProcessing ptfOmniDataProcessing)
        {
            PtfOmniLoanCreateRequest request = null;
            try
            {
                request = _mapper.Map<PtfOmniLoanCreateRequest>(customer);
                request.CustomerInfo.IdsCustomer = GetIdsCustomer(customer);
                request.SaleId = customer.ProductLine == "TSA" ? _ptfOmniConfig.SaleIdTSA : _ptfOmniConfig.SaleIdDSA;
                var quickProcesses = await _ptfOmniMasterDataRepository.FindMetadataAsync("PTF_LOS_MAS_QUICK_PROCESS", "credit_product_id", Int16.Parse(customer.Loan.ProductId));
                if (quickProcesses != null)
                {
                    request.RequestInfo.QuickProcessEnable = !string.IsNullOrEmpty(quickProcesses.Name) ? quickProcesses.Name : "No";
                }
                var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);
                var response = await _restPtfOmniService.CreateLoanAsync(clientId, secretKey, request);

                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    response.Success ? PtfOmniDataProcessingStatus.Done : PtfOmniDataProcessingStatus.Error,
                    request,
                    response,
                    response?.Error?.Message);

                return response;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Content);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Content } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    request,
                    message: ex.Message);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Message } };
            }
        }

        private async Task<PtfOmniResponseModel<object>> UploadDocumentAsync(Customer customer, PtfOmniDataProcessing ptfOmniDataProcessing)
        {
            var payloadModels = new List<PayloadModel>();
            try
            {
                bool isReupload = false;
                bool isSuccess = true;
                if (ptfOmniDataProcessing.Step == DataProcessingStep.PTF_REUPLOAD_DOCUMENT)
                {
                    isReupload = true;
                }
                await ExcUploadDocumentAsync(customer, payloadModels, isReupload);

                // Kiểm tra đã upload hết tài liệu chưa
                foreach (var document in customer.Documents)
                {
                    foreach (var documentDetail in document.Documents)
                    {
                        if (documentDetail.UploadedMedias.Any(x => string.IsNullOrEmpty(x.DocumentId)))
                        {
                            isSuccess = false;
                        }
                    }
                }
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing, isSuccess ? PtfOmniDataProcessingStatus.Done : PtfOmniDataProcessingStatus.Error, payloadModels);

                return new PtfOmniResponseModel<object> { Success = isSuccess };
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    payloadModels,
                    message: ex.Content);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Content } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await UpdatePtfOmniDataProcessingStatusAsync(ptfOmniDataProcessing,
                    PtfOmniDataProcessingStatus.Error,
                    payloadModels,
                    message: ex.Message);
                return new PtfOmniResponseModel<object> { Error = new PtfOmniResponseErrorModel { Message = ex.Message } };
            }
        }


        private IEnumerable<PtfOmniLoanIdsCustomer> GetIdsCustomer(Customer customer)
        {
            var result = new List<PtfOmniLoanIdsCustomer>
            {
                new PtfOmniLoanIdsCustomer
                {
                    IdDocument2 = true,
                    IdDocumentNo = customer.Personal.IdCard,
                    IdIssueCity = customer.Personal.IdCardProvinceId,
                    IdIssueDate = $"{customer.Personal.GetIdCardDate():yyyy-MM-dd}",
                    ExpireDate = $"{customer.Personal.GetIdCardExpiredDate():yyyy-MM-dd}",
                    IdType = "2", //TODO
                }
            };
            if (!string.IsNullOrEmpty(customer.Personal.OldIdCard))
            {
                result.Add(new PtfOmniLoanIdsCustomer
                {
                    IdDocument2 = false,
                    IdDocumentNo = customer.Personal.OldIdCard,
                    IdIssueCity = customer.Personal.OldIdCardProvinceId,
                    IdIssueDate = $"{customer.Personal.GetOldIdCardDate():yyyy-MM-dd}",
                    ExpireDate = $"{customer.Personal.GetOldIdCardExpiredDate():yyyy-MM-dd}",
                    IdType = "2", //TODO
                });
            }

            return result;
        }

        private async Task ExcUploadDocumentAsync(Customer customer, ICollection<PayloadModel> payloads, bool isReupload = false)
        {
            if (string.IsNullOrEmpty(customer.PtfCaseId))
            {
                return;
            }

            var currentMedias = new List<(UploadedMedia uploadedMedia, DocumentUpload documentDetail, GroupDocument document)>();
            var documentToCreate = new List<(UploadedMedia uploadedMedia, DocumentUpload documentDetail, GroupDocument document)>();
            var ptfDocuments = new List<PtfOmniDocumentListResponse>();
            var (clientId, secretKey) = GetAccountSetting(customer.SaleChanelInfo);

            foreach (var document in customer.Documents.Where(x => !string.IsNullOrEmpty(x.GroupCode)))
            {
                foreach (var documentDetail in document.Documents.Where(x => !string.IsNullOrEmpty(x.DocumentCode)))
                {
                    foreach (var uploadedMedia in documentDetail.UploadedMedias.Where(x => !string.IsNullOrEmpty(x.Uri)))
                    {
                        currentMedias.Add((uploadedMedia, documentDetail, document));
                    }
                }
            }
            if (isReupload)
            {
                var documentResponse = await _restPtfOmniService.GetDocumentListAsync(clientId, secretKey, new PtfOmniDocumentListRequest { CaseId = customer.PtfCaseId, Active = true });
                var ptfDocumentsItems = documentResponse.Data.Items;
                if (ptfDocumentsItems != null && ptfDocumentsItems.Count() > 0)
                {
                    ptfDocuments = ptfDocumentsItems.ToList();
                    documentToCreate = currentMedias.Where(x => !ptfDocuments.Any(y => y.DocId == x.uploadedMedia.DocumentId)).ToList();
                    var documentToDelete = ptfDocuments.Where(x => !currentMedias.Any(y => y.uploadedMedia.DocumentId == x.DocId));

                    if (documentToDelete.Any())
                    {
                        var docIds = documentToDelete.Select(x => x.DocId);
                        var result = await _restPtfOmniService.DeleteDocumentsAsync(clientId, secretKey, new PtfOmniDocumentDeleteRequest { CaseId = customer.PtfCaseId, DocIds = docIds });
                    }
                }

            }
            else
            {
                documentToCreate = currentMedias.Where(x => string.IsNullOrEmpty(x.uploadedMedia.DocumentId)).ToList();
            }

            foreach (var (uploadedMedia, documentDetail, document) in documentToCreate)
            {
                var result = await PTFUploadDocumentAsync(clientId, secretKey, customer.PtfCaseId, document.GroupCode, documentDetail.DocumentCode, uploadedMedia.Uri);
                uploadedMedia.DocumentId = result?.Data?.DocID;

                payloads.Add(new PayloadModel
                {
                    Payload = JsonConvert.SerializeObject(new { customer.PtfCaseId, document.GroupCode, documentDetail.DocumentCode, uploadedMedia.Uri }),
                    Response = JsonConvert.SerializeObject(result)
                });

                if (!result.Success)
                {
                    throw new ArgumentException(result.Error?.Message);
                }
            }

        }

        private async Task<PtfOmniResponseModel<PtfOmniDocumentUploadResponse>> PTFUploadDocumentAsync(string clientId, string secretKey, string caseId, string documentCategory, string documentType, string url)
        {
            string content = "";
            try
            {
                var client = new RestClient($"{_ptfOmniConfig.Host}/documents/upload-multipart");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-CLIENT-ID", clientId);
                request.AddHeader("X-SECRET-API", secretKey);
                request.AddParameter("caseId", caseId);
                request.AddParameter("documentCategory", documentCategory);
                request.AddParameter("documentType", documentType);

                var file = await _storageService.GetObjectAsync(url);
                request.AddFileBytes("file", file.Bytes, file.FileName);
                IRestResponse response = await client.ExecuteAsync(request);
                content = response.Content;
                return JsonConvert.DeserializeObject<PtfOmniResponseModel<PtfOmniDocumentUploadResponse>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PtfOmniResponseModel<PtfOmniDocumentUploadResponse>
                {
                    Data = new PtfOmniDocumentUploadResponse { DocID = string.Empty },
                    Error = new PtfOmniResponseErrorModel { Message = $"{ex.Message}; {content}" }
                };
            }
        }

        private async Task UpdatePtfOmniDataProcessingStatusAsync(PtfOmniDataProcessing ptfOmniDataProcessing, PtfOmniDataProcessingStatus status,
            object request = null, object result = null, string message = null)
        {
            string payload = request != null ? JsonConvert.SerializeObject(request) : null;
            string response = result != null ? JsonConvert.SerializeObject(result) : null;
            var payloadModel = new PayloadModel
            {
                Payload = payload,
                Response = response,
                CreateDate = DateTime.Now,
                Message = message
            };
            var payloads = ptfOmniDataProcessing.Payloads?.ToList() ?? new List<PayloadModel>();
            payloads.Add(payloadModel);

            var update = Builders<PtfOmniDataProcessing>.Update
                    .Set(x => x.Status, status)
                    .Set(x => x.Payloads, payloads)
                    .Set(x => x.ModifiedDate, DateTime.Now);

            await _ptmOmniDataProcessingRepository.UpdateOneAsync(x => x.Id == ptfOmniDataProcessing.Id, update);

            ptfOmniDataProcessing.Payloads = payloads;
        }

        private async Task UpdatePtfOmniDataProcessingStatusAsync(PtfOmniDataProcessing ptfOmniDataProcessing, PtfOmniDataProcessingStatus status)
        {
            var update = Builders<PtfOmniDataProcessing>.Update
                    .Set(x => x.Status, status)
                    .Set(x => x.ModifiedDate, DateTime.Now);

            await _ptmOmniDataProcessingRepository.UpdateOneAsync(x => x.Id == ptfOmniDataProcessing.Id, update);
        }

        private async Task UpdatePtfOmniDataProcessingStatusAsync(PtfOmniDataProcessing ptfOmniDataProcessing, PtfOmniDataProcessingStatus status, IEnumerable<PayloadModel> payloadModels)
        {
            var payloads = ptfOmniDataProcessing.Payloads?.ToList() ?? new List<PayloadModel>();
            payloads.AddRange(payloadModels);

            var update = Builders<PtfOmniDataProcessing>.Update
                    .Set(x => x.Status, status)
                    .Set(x => x.Payloads, payloads)
                    .Set(x => x.ModifiedDate, DateTime.Now);

            await _ptmOmniDataProcessingRepository.UpdateOneAsync(x => x.Id == ptfOmniDataProcessing.Id, update);

            ptfOmniDataProcessing.Payloads = payloads;
        }

        private async Task UpdateStepAsync(string ptfOmniDataProcessingId, string step)
        {
            var update = Builders<PtfOmniDataProcessing>.Update
                    .Set(x => x.Step, step)
                    .Set(x => x.ModifiedDate, DateTime.Now);

            await _ptmOmniDataProcessingRepository.UpdateOneAsync(x => x.Id == ptfOmniDataProcessingId, update);
        }

        private (string, string) GetAccountSetting(SaleChanelInfo saleChanelInfo)
        {
            var clientId = string.IsNullOrEmpty(saleChanelInfo?.SaleChanelConfigUserInfo?.ClientID) ? _ptfOmniConfig.ClientId : saleChanelInfo.SaleChanelConfigUserInfo.ClientID;
            var secretKey = string.IsNullOrEmpty(saleChanelInfo?.SaleChanelConfigUserInfo?.SecretKey) ? _ptfOmniConfig.SecretId : saleChanelInfo.SaleChanelConfigUserInfo.SecretKey;
            return (clientId, secretKey);
        }
    }
}
