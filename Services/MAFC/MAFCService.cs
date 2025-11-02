using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Services.Files;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMafcService
    {
        Task<GetMafcDetailResponse> CreateAsync(CreateMafcStep1Request createMafcRequest);
        Task<GetMafcDetailResponse> CreateAsync(CreateMafcStep1WebRequest createMafcRequest);
        Task UpdateStep1Async(string id, UpdateMafcStep1Request updateMafcRequest);
        Task UpdateStep1WebAsync(string id, UpdateMafcStep1WebRequest updateMafcRequest);
        Task UpdateStep2Async(string id, UpdateMafcStep2Request updateMafcStep2Request);
        Task UpdateStep3Async(string id, UpdateMafcStep3Request updateMafcStep3Request);
        Task UpdateStep3WebAsync(string id, UpdateMafcStep3WebRequest updateMafcStep3Request);
        Task UpdateStep4Async(string id, UpdateMafcStep4Request updateMafcStep4Request);
        Task UpdateStep5Async(string id, UpdateMafcStep5Request updateMafcStep5Request);
        Task UpdateStep6Async(string id, UpdateMafcStep6Request updateMafcStep6Request);
        Task UpdateRecordFileAsync(string id, UpdateMafcRecordFileRequest updateMafcStep6Request);
        Task<PagingResponse<GetMafcResponse>> GetAsync(GetMafcResquest getMafcResquest);
        Task<PagingResponse<GetOldMafcResponse>> GetOldAppAsync(GetOldAppMafcResquest getMafcResquest);
        Task<GetMafcDetailResponse> GetDetailAsync(string id);
    }
    public class MafcService : IMafcService, IScopedLifetime
    {
        private readonly ILogger<MafcService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly ChecklistService _checklistService;
        private readonly IFileService _fileService;
        private readonly DataMAFCProcessingServices _dataMAFCProcessingServices;
        private readonly CRM.DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<MAFCSaleOffice> _saleOfficeRepository;
        private readonly IMAFCSaleOfficeService _mafcSaleOfficeService;
        private readonly IMAFCDataEntryService _mafcDataEntryService;
        private readonly IUserServices _userServices;
        private readonly IMongoRepository<POS> _posRepository;


        public MafcService(
            ILogger<MafcService> logger,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            ICustomerRepository customerRepository,
            IHistoryDomainService historyDomainService,
            ChecklistService checklistService,
            IFileService fileService,
            DataMAFCProcessingServices dataMAFCProcessingServices,
            CRM.DataCRMProcessingServices dataCRMProcessingServices,
            IMAFCSaleOfficeService mafcSaleOfficeService,
            IMAFCDataEntryService mafcDataEntryService,
            IMongoRepository<MAFCSaleOffice> saleOfficeRepository,
            IUserServices userServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _historyDomainService = historyDomainService;
            _checklistService = checklistService;
            _fileService = fileService;
            _dataMAFCProcessingServices = dataMAFCProcessingServices;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _saleOfficeRepository = saleOfficeRepository;
            _mafcSaleOfficeService = mafcSaleOfficeService;
            _mafcDataEntryService = mafcDataEntryService;
            _userServices = userServices;
            _posRepository = posRepository;
        }


        public async Task<GetMafcDetailResponse> CreateAsync(CreateMafcStep1Request createMafcRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createMafcRequest.Personal.IdCard);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createMafcRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.UserName = user.UserName;

                customer.Creator = _userLoginService.GetUserId();
                if (!string.IsNullOrEmpty(createMafcRequest.OldCustomerId) && createMafcRequest.CustomerTypeId == "OLD")
                {
                    var oldCus = await _customerRepository.FindByIdAsync(createMafcRequest.OldCustomerId);
                    if (oldCus != null)
                    {
                        customer.OldCustomer = _mapper.Map<OldCustomer>(oldCus);
                    }
                }
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                var response = _mapper.Map<GetMafcDetailResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetMafcDetailResponse> CreateAsync(CreateMafcStep1WebRequest createMafcRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createMafcRequest.Personal.IdCard);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createMafcRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.UserName = user.UserName;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                ValidateMaritalStatus(customer, createMafcRequest.Spouse);

                customer.Creator = _userLoginService.GetUserId();
                if (!string.IsNullOrEmpty(createMafcRequest.OldCustomerId) && createMafcRequest.CustomerTypeId == "OLD")
                {
                    var oldCus = await _customerRepository.FindByIdAsync(createMafcRequest.OldCustomerId);
                    if (oldCus != null)
                    {
                        customer.OldCustomer = _mapper.Map<OldCustomer>(oldCus);
                    }
                }
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                var response = _mapper.Map<GetMafcDetailResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep1Async(string id, UpdateMafcStep1Request updateMafcRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateMafcRequest.Personal.IdCard, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();
                _mapper.Map(updateMafcRequest, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep1Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task UpdateStep1WebAsync(string id, UpdateMafcStep1WebRequest updateMafcRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateMafcRequest.Personal.IdCard, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMafcRequest, customer);
                ValidateMaritalStatus(customer, updateMafcRequest.Spouse);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep1WebAsync), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep2Async(string id, UpdateMafcStep2Request updateMafcStep2Request)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();
                _mapper.Map(updateMafcStep2Request, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep2Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task UpdateStep3Async(string id, UpdateMafcStep3Request updateMafcStep3Request)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();
                if (customer.Loan?.ProductId != updateMafcStep3Request.Loan?.ProductId && string.IsNullOrEmpty(customer.ContractCode))
                {
                    customer.Documents = null;
                }
                _mapper.Map(updateMafcStep3Request, customer);

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

        public async Task UpdateStep3WebAsync(string id, UpdateMafcStep3WebRequest updateMafcStep3Request)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();
                if (customer.Loan?.ProductId != updateMafcStep3Request.Loan?.ProductId && string.IsNullOrEmpty(customer.ContractCode))
                {
                    customer.Documents = null;
                }
                _mapper.Map(updateMafcStep3Request, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                if (customer.Documents?.Any() != true && !string.IsNullOrEmpty(customer.Loan?.CategoryId))
                {
                    if (string.IsNullOrEmpty(customer.SaleInfo.MAFCId))
                    {
                        var mafcInfo = await _mafcSaleOfficeService.GetByMAFCCodeAsync(customer.SaleInfo.MAFCCode);
                        if (mafcInfo != null)
                        {
                            customer.SaleInfo.MAFCId = mafcInfo.InspectorId;
                            customer.SaleInfo.MAFCCode = mafcInfo.InspectorName;
                            customer.SaleInfo.MAFCName = customer.SaleInfo.Name;
                            await _customerRepository.ReplaceOneAsync(customer);
                        }
                    }
                    ChecklistModel result = _checklistService.GetCheckListByCategoryId(customer.Loan.CategoryId);
                    var dnGroup = result?.Checklist?.FirstOrDefault(x => x.GroupId == 1);
                    if (dnGroup != null)
                    {
                        var file = await _fileService.GenarateDNFile(customer.Id);
                        List<UploadedMedia> dn = new List<UploadedMedia>()
                            {
                                new UploadedMedia () {
                                    Name = file.FileName,
                                    Type = "pdf",
                                    Uri = file.AbsolutePath
                                }
                            };
                        dnGroup.Documents.First().UploadedMedias = dn;
                        customer.Documents = result.Checklist;
                        await _customerRepository.ReplaceOneAsync(customer);
                    }
                }
                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep3WebAsync), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep4Async(string id, UpdateMafcStep4Request updateMafcStep4Request)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMafcStep4Request, customer);

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

        public async Task UpdateStep5Async(string id, UpdateMafcStep5Request updateMafcStep5Request)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMafcStep5Request, customer);
                ValidateMaritalStatus(customer, updateMafcStep5Request.Spouse);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep5Async), valueBefore, customer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep6Async(string id, UpdateMafcStep6Request updateMafcStep6Request)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMafcStep6Request, customer);
                if (customer.RecordFile == null && valueBefore.RecordFile != null)
                {
                    customer.RecordFile = valueBefore.RecordFile;
                }

                if (updateMafcStep6Request.Status == CustomerStatus.SUBMIT)
                {
                    customer.Status = CustomerStatus.SUBMIT;
                    if (customer.MAFCId != 0)
                    {
                        customer.Status = CustomerStatus.SENDING;
                        var process = new DataMAFCProcessingModel
                        {
                            CustomerId = customer.Id,
                            Status = DataProcessingStatus.IN_PROGRESS,
                            Step = DataProcessingStep.PPH
                        };
                        if (customer.Result.ReturnStatus == "QDE" || customer.Result.ReturnStatus == "BDE")
                        {
                            process.Status = DataProcessingStatus.IN_PROGRESS;
                            process.Step = DataProcessingStep.DEU;
                        }
                        await _dataMAFCProcessingServices.CreateOneAsync(process);
                        BackgroundJob.Enqueue<IMAFCDataEntryService>(x => x.ProcessRecordAsync(customer));
                    }
                }

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep6Async), valueBefore, customer);

                if (customer.Status == CustomerStatus.SUBMIT)
                {
                    await SubmitCustomerAsync(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateRecordFileAsync(string id, UpdateMafcRecordFileRequest updateMafcStep6Request)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMafcStep6Request, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                customer.Status = CustomerStatus.SENDING;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateRecordFileAsync), valueBefore, customer);

                var model = new DataMAFCProcessingModel
                {
                    CustomerId = customer.Id,
                    Status = DataCRMProcessingStatus.InProgress
                };
                await _dataMAFCProcessingServices.CreateOneAsync(model);
                BackgroundJob.Enqueue<IMAFCDataEntryService>(x => x.ProcessRecordAsync(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetMafcDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadMafcManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMafcManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadMafcManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadMafcManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadMafcManagement_ViewAll);

                Expression<Func<Customer, bool>> filter = x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    (!filterByCreatorIds.Any() || filterByCreatorIds.Contains(x.Creator));
                var customer = await _customerRepository.FindOneAsync(filter);

                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var result = _mapper.Map<GetMafcDetailResponse>(customer);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetMafcResponse>> GetAsync(GetMafcResquest getMafcResquest)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadMafcManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMafcManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadMafcManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadMafcManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadMafcManagement_ViewAll);

                var customerFilter = new CustonerFilterDto
                {
                    GreenType = GreenType.GreenA,
                    CreatorIds = filterByCreatorIds,
                    Status = getMafcResquest.Status,
                    CustomerName = getMafcResquest.CustomerName,
                    Sale = getMafcResquest.Sale,
                    PosManager = getMafcResquest.PosManager,
                    TeamLead = getMafcResquest.TeamLead,
                    Asm = getMafcResquest.Asm,
                    FromDate = getMafcResquest.GetFromDate(),
                    ToDate = getMafcResquest.GetToDate(),
                    ProductLine = getMafcResquest.ProductLine,
                    ReturnStatus = getMafcResquest.ReturnStatus,
                    PageIndex = getMafcResquest.PageIndex,
                    PageSize = getMafcResquest.PageSize,
                };
                var customers = await _customerRepository.GetAsync<GetMafcResponse>(customerFilter);
                var total = await _customerRepository.CountAsync(customerFilter);

                var result = new PagingResponse<GetMafcResponse>
                {
                    TotalRecord = total,
                    Data = customers
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetOldMafcResponse>> GetOldAppAsync(GetOldAppMafcResquest getMafcResquest)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var customerFilter = new CustonerFilterDto
                {
                    GreenType = GreenType.GreenA,
                    CreatorIds = new List<string>() { userId },
                    CustomerName = getMafcResquest.CustomerName,
                    HasMafcId = true,
                    PageIndex = getMafcResquest.PageIndex,
                    PageSize = getMafcResquest.PageSize,
                };
                var customers = await _customerRepository.GetAsync<Customer>(customerFilter);
                var dataResponse = _mapper.Map<IEnumerable<GetOldMafcResponse>>(customers);
                var total = await _customerRepository.CountAsync(customerFilter);
                var result = new PagingResponse<GetOldMafcResponse>
                {
                    TotalRecord = total,
                    Data = dataResponse
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> IsExistedAsync(string idCard, string id = null)
        {
            DateTime datefrom = DateTime.Now.AddDays(-15);
            string[] listOfStatus = { CustomerStatus.SUBMIT, CustomerStatus.PROCESSING, CustomerStatus.SENDING, CustomerStatus.REVIEW, CustomerStatus.APPROVE };
            var customerExisted = await _customerRepository.FindOneAsync(x =>
                x.Id != id &&
                !x.IsDeleted &&
                listOfStatus.Contains(x.Status) &&
                x.ModifiedDate > datefrom &&
                x.GreenType == GreenType.GreenA &&
                x.Personal.IdCard == idCard);
            return customerExisted != null;
        }

        private async Task SubmitCustomerAsync(Customer customer)
        {
            var currUser = await _userRepository.FindOneAsync(x => x.Id == customer.Creator);

            // Update to CRM
            var dataCRMProcessing = new DataCRMProcessing
            {
                CustomerId = customer.Id,
                Status = DataCRMProcessingStatus.InProgress,
                LeadSource = LeadSourceType.MC
            };
            _dataCRMProcessingServices.InsertOne(dataCRMProcessing);

            var mafcInfo = await _saleOfficeRepository.FindOneAsync(x => x.InspectorName == currUser.MAFCCode);
            if (mafcInfo != null)
            {
                var model = new DataMAFCProcessingModel
                {
                    CustomerId = customer.Id,
                    Status = DataCRMProcessingStatus.InProgress
                };
                await _dataMAFCProcessingServices.CreateOneAsync(model);
                customer.Status = CustomerStatus.SENDING;
                var customerBefore = await _customerRepository.FindByIdAsync(customer.Id);
                await _customerRepository.ReplaceOneAsync(customer);
                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitCustomerAsync), customerBefore, customer);

                BackgroundJob.Enqueue<IMAFCDataEntryService>(x => x.ProcessRecordAsync(customer));
            }
            else
            {
                customer.RecordFileBackup = customer.RecordFile;
                customer.RecordFile = null;
                customer.Status = CustomerStatus.REVIEW;

                var customerBefore = await _customerRepository.FindByIdAsync(customer.Id);
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitCustomerAsync), customerBefore, customer);
            }
        }
        private void ValidateMaritalStatus(Customer customer, MafcRefereeDto spouse)
        {
            if (!string.IsNullOrEmpty(customer.Personal.MaritalStatusId))
            {
                if (customer.Personal.MaritalStatusId == "M")
                {
                    if (string.IsNullOrEmpty(spouse?.Name)
                        || string.IsNullOrEmpty(spouse?.IdCard))
                    {
                        throw new ArgumentException(string.Format(Message.REQUIRED, "Thông tin vợ/chồng"));
                    }
                    if (customer.Personal.Title == "MR.")
                    {
                        customer.Spouse.Title = "MRS.";
                        customer.Spouse.TitleId = "MRS.";
                    }
                    else
                    {
                        customer.Spouse.Title = "MR.";
                        customer.Spouse.TitleId = "MR.";
                    }
                    customer.Spouse.Relationship = "Vợ chồng";
                    customer.Spouse.RelationshipId = "WH";
                }
            }
            else
            {
                throw new ArgumentException(string.Format(Message.REQUIRED, "Tình trạng hôn nhân"));
            }
        }
    }
}
