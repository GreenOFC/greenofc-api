using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public interface IMcApplicationService
    {
        Task<GetMcDetailResponse> CreateAsync(CreateMcRequest createMcRequest);
        Task<GetMcDetailResponse> CreateStep1Async(CreateMcStep1Request createMcRequest);
        Task<GetMcDetailResponse> CreateStep1Async(CreateMcLeadStep1Request createMcLeadRequest);
        Task UpdateAsync(string id, UpdateMcRequest updateMcRequest);
        Task UpdateStep1Async(string id, UpdateMcStep1Request updateMcRequest);
        Task UpdateStep1Async(string id, UpdateMcLeadStep1Request updateMcLeadRequest);
        Task UpdateStep2Async(string id, UpdateMcStep2Request updateMcRequest);
        Task UpdateStep2Async(string id, UpdateMcLeadStep2Request updateMcLeadStep2Request);
        Task UpdateStep3Async(string id, UpdateMcStep3Request updateMcRequest);
        Task UpdateStep4Async(string id, UpdateMcStep4Request updateMcRequest);
        Task UpdateStep5Async(string id, UpdateMcStep5Request updateMcRequest);
        Task UpdateStatusAsync(string id, UpdateMcStatusRequest updateMcRequest);
        Task<GetMcDetailResponse> GetDetailAsync(string id);
        Task<PagingResponse<GetMcResponse>> GetAsync(GetMcResquest getMcResquest);
    }

    public class McApplicationService : IMcApplicationService, IScopedLifetime
    {
        private readonly ILogger<McApplicationService> _logger;
        private readonly DataMCProcessingServices _dataMCProcessingServices;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly IRestMCService _restMCService;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IMongoRepository<User> _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly CRM.DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly IMcDomainService _mcDomainService;
        private readonly IUserServices _userServices;
        private readonly IMongoRepository<POS> _posRepository;

        public McApplicationService(
            ILogger<McApplicationService> logger,
            DataMCProcessingServices dataMCProcessingServices,
            CustomerDomainServices customerDomainServices,
            IRestMCService restMCService,
            IMapper mapper,
            IUserLoginService userLoginService,
            IMongoRepository<User> userRepository,
            ICustomerRepository customerRepository,
            CRM.DataCRMProcessingServices dataCRMProcessingServices,
            INotificationRepository notificationRepository,
            IHistoryDomainService historyDomainService,
            IMcDomainService mcDomainService,
            IUserServices userServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _dataMCProcessingServices = dataMCProcessingServices;
            _restMCService = restMCService;
            _customerDomainServices = customerDomainServices;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _notificationRepository = notificationRepository;
            _historyDomainService = historyDomainService;
            _mcDomainService = mcDomainService;
            _userServices = userServices;
            _posRepository = posRepository;
        }

        public async Task<GetMcDetailResponse> CreateAsync(CreateMcRequest createMcRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createMcRequest.Personal.IdCard);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createMcRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.UserName = user.UserName;

                customer.Creator = _userLoginService.GetUserId();
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                var response = _mapper.Map<GetMcDetailResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetMcDetailResponse> CreateStep1Async(CreateMcStep1Request createMcRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createMcRequest.Personal.IdCard);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindOneAsync(x => !x.IsDeleted && x.Id == _userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createMcRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.UserName = user.UserName;

                customer.Creator = _userLoginService.GetUserId();
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateStep1Async), valueAfter: customer);

                var response = _mapper.Map<GetMcDetailResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetMcDetailResponse> CreateStep1Async(CreateMcLeadStep1Request createMcLeadRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createMcLeadRequest.Personal.IdCard);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var userId = _userLoginService.GetUserId();
                var user = await _userRepository.FindByIdAsync(userId);

                var customer = _mapper.Map<Customer>(createMcLeadRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.UserName = user.UserName;
                customer.Creator = userId;
                customer.Documents = McCheckListConst.Documents;

                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateStep1Async), valueAfter: customer);

                var response = _mapper.Map<GetMcDetailResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateMcRequest updateMcRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateMcRequest.Personal.IdCard, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                // Xóa Documents khi thay đổi sản phẩm và MCId == 0
                if (customer.Loan?.ProductId != updateMcRequest.Loan?.ProductId && customer.MCId == 0)
                {
                    customer.Documents = null;
                }
                _mapper.Map(updateMcRequest, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateAsync), valueBefore, customer);

                if (customer.Status == CustomerStatus.SUBMIT)
                {
                    await _customerDomainServices.SubmitCustomerAsync(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep1Async(string id, UpdateMcStep1Request updateMcRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateMcRequest.Personal.IdCard, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMcRequest, customer);

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

        public async Task UpdateStep1Async(string id, UpdateMcLeadStep1Request updateMcLeadRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateMcLeadRequest.Personal.IdCard, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMcLeadRequest, customer);

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

        public async Task UpdateStep2Async(string id, UpdateMcStep2Request updateMcRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                string companyTaxCode = customer.Working?.CompanyTaxCode;
                _mapper.Map(updateMcRequest, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                customer.Working.CompanyTaxCode = companyTaxCode;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep2Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep2Async(string id, UpdateMcLeadStep2Request updateMcLeadStep2Request)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                customer.Documents = _mapper.Map<IEnumerable<GroupDocument>>(updateMcLeadStep2Request.Documents);
                if (updateMcLeadStep2Request.Status == CustomerStatus.SUBMIT)
                {
                    customer.Status = CustomerStatus.SUBMIT;
                }
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                var update = Builders<Customer>.Update
                    .Set(x => x.Documents, customer.Documents)
                    .Set(x => x.Status, customer.Status)
                    .Set(x => x.Modifier, customer.Modifier)
                    .Set(x => x.ModifiedDate, customer.ModifiedDate);
                await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);

                // Update to CRM
                var dataCRMProcessing = new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    Status = DataCRMProcessingStatus.InProgress,
                    LeadSource = LeadSourceType.MC
                };
                _dataCRMProcessingServices.InsertOne(dataCRMProcessing);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep2Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep3Async(string id, UpdateMcStep3Request updateMcRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                // Xóa Documents khi thay đổi sản phẩm và MCId == 0
                if (customer.Loan?.ProductId != updateMcRequest.Loan?.ProductId && customer.MCId == 0)
                {
                    customer.Documents = null;
                }

                _mapper.Map(updateMcRequest, customer);

                customer.Working.CompanyTaxCode = updateMcRequest.CompanyTaxCode;
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                if (customer.Documents?.Any() != true)
                {
                    var result = await _mcDomainService.CheckListAsync(id);
                    customer.Documents = _mapper.Map<IEnumerable<GroupDocument>>(result.CheckList);
                }
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep3Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep4Async(string id, UpdateMcStep4Request updateMcRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateMcRequest, customer);

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

        public async Task UpdateStep5Async(string id, UpdateMcStep5Request updateMcDocumentRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                if (customer.ReturnDocuments != null)
                {
                    customer.ReturnDocuments = _mapper.Map<IEnumerable<GroupDocument>>(updateMcDocumentRequest.ReturnDocuments);
                    customer.CaseNote = updateMcDocumentRequest.CaseNote;
                }
                else
                {
                    customer.Documents = _mapper.Map<IEnumerable<GroupDocument>>(updateMcDocumentRequest.Documents);
                }

                if (updateMcDocumentRequest.RecordFile != null)
                {
                    customer.RecordFile = _mapper.Map<UploadedMedia>(updateMcDocumentRequest.RecordFile);
                }

                if (updateMcDocumentRequest.Status == CustomerStatus.SUBMIT)
                {
                    customer.Status = CustomerStatus.SENDING;
                    var dataMCProcessing = new DataMCProcessing
                    {
                        CustomerId = customer.Id,
                        Status = DataCRMProcessingStatus.InProgress
                    };
                    _dataMCProcessingServices.CreateOne(dataMCProcessing);
                    // send case note
                    if (!string.IsNullOrEmpty(customer.MCAppId)
                        && customer.Result?.ReturnStatus == "SALE Đang chờ nhập liệu bổ sung - Return"
                        && !string.IsNullOrEmpty(customer.CaseNote))
                    {
                        var mCSendCaseNoteRequestDto = new MCSendCaseNoteRequestDto
                        {
                            AppNumber = customer.MCAppnumber,
                            NoteContent = customer.CaseNote
                        };

                        await _restMCService.SendCaseNoteAsync(mCSendCaseNoteRequestDto);
                    }
                }

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                // Update to CRM
                var dataCRMProcessing = new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    Status = DataCRMProcessingStatus.InProgress,
                    LeadSource = LeadSourceType.MC
                };
                _dataCRMProcessingServices.InsertOne(dataCRMProcessing);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep5Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStatusAsync(string id, UpdateMcStatusRequest updateMcStatusRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.ViewAllMc) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                string sender = string.Empty,
                    userName = string.Empty,
                    message = string.Empty,
                    type = string.Empty;

                var currUser = await _userRepository.FindOneAsync(x => x.UserName == customer.UserName);
                if (!string.IsNullOrEmpty(currUser?.TeamLeadInfo?.Id))
                {
                    var teamLeadUser = await _userRepository.FindByIdAsync(currUser.TeamLeadInfo.Id);
                    sender = teamLeadUser?.UserName;
                }
                customer.Status = updateMcStatusRequest.Status;
                if (!string.IsNullOrEmpty(updateMcStatusRequest.Reason))
                {
                    customer.Result.Reason = updateMcStatusRequest.Reason;
                }
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStatusAsync), valueBefore, customer);


                if (customer.Status.ToUpper() == CustomerStatus.REJECT)
                {
                    userName = customer.UserName;
                    type = NotificationType.Reject;
                    message = string.Format(Message.NotificationReject, sender, customer.Personal.Name);
                }
                else if (customer.Status.ToUpper() == CustomerStatus.APPROVE)
                {
                    // send data to MC
                    var dataMCProcessing = new DataMCProcessing
                    {
                        CustomerId = customer.Id,
                        Status = DataCRMProcessingStatus.InProgress
                    };
                    _dataMCProcessingServices.CreateOne(dataMCProcessing);

                    userName = customer.UserName;
                    type = NotificationType.Approve;
                    message = string.Format(Message.TeamLeadApprove, sender, customer.Personal.Name);
                }

                // Update to CRM
                var dataCRMProcessing = new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    Status = DataCRMProcessingStatus.InProgress,
                    LeadSource = LeadSourceType.MC
                };
                _dataCRMProcessingServices.InsertOne(dataCRMProcessing);

                if (message != "")
                {
                    var objNoti = new Notification
                    {
                        GreenType = customer.GreenType,
                        RecordId = customer.Id,
                        Type = type,
                        UserName = userName,
                        UserId = currUser.Id,

                        Message = message,
                    };
                    await _notificationRepository.InsertOneAsync(objNoti);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetMcDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadMcManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMcManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadMcManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadMcManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadMcManagement_ViewAll);

                Expression<Func<Customer, bool>> filter = x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    (!filterByCreatorIds.Any() || filterByCreatorIds.Contains(x.Creator));
                var customer = await _customerRepository.FindOneAsync(filter);

                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var result = _mapper.Map<GetMcDetailResponse>(customer);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetMcResponse>> GetAsync(GetMcResquest getMcResquest)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadMcManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadMcManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadMcManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadMcManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadMcManagement_ViewAll);

                var customerFilter = new CustonerFilterDto
                {
                    GreenType = GreenType.GreenC,
                    CreatorIds = filterByCreatorIds,
                    Status = getMcResquest.Status,
                    CustomerName = getMcResquest.CustomerName,
                    Sale = getMcResquest.Sale,
                    PosManager = getMcResquest.PosManager,
                    TeamLead = getMcResquest.TeamLead,
                    FromDate = getMcResquest.GetFromDate(),
                    ToDate = getMcResquest.GetToDate(),
                    ProductLine = getMcResquest.ProductLine,
                    ReturnStatus = getMcResquest.ReturnStatus,
                    PageIndex = getMcResquest.PageIndex,
                    PageSize = getMcResquest.PageSize,
                };
                var customers = await _customerRepository.GetAsync<GetMcResponse>(customerFilter);
                var total = await _customerRepository.CountAsync(customerFilter);

                var result = new PagingResponse<GetMcResponse>
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

        private async Task<bool> IsExistedAsync(string idCard, string id = null)
        {
            if (string.IsNullOrEmpty(idCard) && id == null)
            {
                return false;
            }
            DateTime datefrom = DateTime.Now.AddDays(-15);
            string[] listOfStatus = { CustomerStatus.SUBMIT, CustomerStatus.PROCESSING, CustomerStatus.APPROVE, CustomerStatus.RETURN, CustomerStatus.SENDING, CustomerStatus.REVIEW };
            var customerExisted = await _customerRepository.FindOneAsync(x =>
                x.Id != id &&
                !x.IsDeleted &&
                listOfStatus.Contains(x.Status) &&
                x.ModifiedDate > datefrom &&
                x.GreenType == GreenType.GreenC &&
                x.Personal.IdCard == idCard);
            return customerExisted != null;
        }
    }
}
