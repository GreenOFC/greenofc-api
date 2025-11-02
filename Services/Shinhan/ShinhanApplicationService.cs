using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Shinhan;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Shinhan
{
    public interface IShinhanApplicationService
    {
        Task<GetShinhanDetailResponse> CreateAsync(CreateShinhanRequest createShinhanRequest);
        Task<GetShinhanDetailResponse> CreateStep1Async(CreateShinhanStep1Request createShinhanRequest);
        Task UpdateAsync(string id, UpdateShinhanRequest updateShinhanRequest);
        Task UpdateStep1Async(string id, UpdateShinhanStep1Request updateShinhanRequest);
        Task UpdateStep2Async(string id, UpdateShinhanStep2Request updateShinhanRequest);
        Task UpdateStep3Async(string id, UpdateShinhanStep3Request updateShinhanRequest);
        Task UpdateStep4Async(string id, UpdateShinhanStep4Request updateShinhanRequest);
        Task UpdateStep5Async(string id, UpdateShinhanStep5Request updateShinhanRequest);
        Task<GetShinhanDetailResponse> GetDetailAsync(string id);
        Task<PagingResponse<GetShinhanResponse>> GetAsync(GetShinhanResquest getShinhanResquest);
    }

    public class ShinhanApplicationService : IShinhanApplicationService, IScopedLifetime
    {
        private readonly ILogger<ShinhanApplicationService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IMongoRepository<User> _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly CRM.DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly IUserServices _userServices;
        private readonly ChecklistService _checklistService;

        public ShinhanApplicationService(
            ILogger<ShinhanApplicationService> logger,
            CustomerDomainServices customerDomainServices,
            IMapper mapper,
            IUserLoginService userLoginService,
            IMongoRepository<User> userRepository,
            ICustomerRepository customerRepository,
            CRM.DataCRMProcessingServices dataCRMProcessingServices,
            IHistoryDomainService historyDomainService,
            ChecklistService checklistService,
            IUserServices userServices)
        {
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _historyDomainService = historyDomainService;
            _userServices = userServices;
            _checklistService = checklistService;
        }

        public async Task<GetShinhanDetailResponse> CreateAsync(CreateShinhanRequest createShinhanRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createShinhanRequest.Personal.IdCard);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindOneAsync(x => !x.IsDeleted && x.Id == _userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createShinhanRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.UserName = user.UserName;

                customer.Creator = _userLoginService.GetUserId();
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                var response = _mapper.Map<GetShinhanDetailResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateShinhanRequest updateShinhanRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateShinhanRequest.Personal.IdCard, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                customer.Documents = customer.Loan?.ProductId == updateShinhanRequest.Loan?.ProductId ? customer.Documents : null;
                _mapper.Map(updateShinhanRequest, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateAsync), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetShinhanDetailResponse> CreateStep1Async(CreateShinhanStep1Request createShinhanRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createShinhanRequest.Personal.IdCard);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindOneAsync(x => !x.IsDeleted && x.Id == _userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createShinhanRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.UserName = user.UserName;

                customer.Creator = _userLoginService.GetUserId();
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateStep1Async), valueAfter: customer);

                var response = _mapper.Map<GetShinhanDetailResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep1Async(string id, UpdateShinhanStep1Request updateShinhanRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateShinhanRequest.Personal.IdCard, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if (!_userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateShinhanRequest, customer);

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

        public async Task UpdateStep2Async(string id, UpdateShinhanStep2Request updateShinhanRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();
                _mapper.Map(updateShinhanRequest, customer);

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

        public async Task UpdateStep3Async(string id, UpdateShinhanStep3Request updateShinhanRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateShinhanRequest, customer);

                if (customer.Documents?.Any() != true)
                {
                    ChecklistModel result = _checklistService.GetCheckListByCategoryId("Shinhan");
                    customer.Documents = _mapper.Map<IEnumerable<GroupDocument>>(result.Checklist);
                }

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

        public async Task UpdateStep4Async(string id, UpdateShinhanStep4Request updateShinhanRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateShinhanRequest, customer);

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

        public async Task UpdateStep5Async(string id, UpdateShinhanStep5Request updateShinhanDocumentRequest)
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

                if (!_userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll) && _userLoginService.GetUserId() != customer.Creator)
                {
                    throw new ArgumentException(Message.UNAUTHORIZED);
                }

                var valueBefore = customer.Clone();

                if (customer.ReturnDocuments != null)
                {
                    customer.ReturnDocuments = _mapper.Map<IEnumerable<GroupDocument>>(updateShinhanDocumentRequest.ReturnDocuments);
                    customer.CaseNote = updateShinhanDocumentRequest.CaseNote;
                }
                else
                {
                    customer.Documents = _mapper.Map<IEnumerable<GroupDocument>>(updateShinhanDocumentRequest.Documents);
                }

                if (updateShinhanDocumentRequest.RecordFile != null)
                {
                    customer.RecordFile = _mapper.Map<UploadedMedia>(updateShinhanDocumentRequest.RecordFile);
                }

                if (updateShinhanDocumentRequest.Status == CustomerStatus.SUBMIT)
                {
                    customer.Status = CustomerStatus.PROCESSING;
                    // Update to CRM
                    var dataCRMProcessing = new DataCRMProcessing
                    {
                        CustomerId = customer.Id,
                        Status = DataCRMProcessingStatus.InProgress,
                        LeadSource = LeadSourceType.MC
                    };
                    _dataCRMProcessingServices.InsertOne(dataCRMProcessing);
                }

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
        public async Task<GetShinhanDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadShinhanManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadShinhanManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadShinhanManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadShinhanManagement_ViewAll);

                Expression<Func<Customer, bool>> filter = x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    (!filterByCreatorIds.Any() || filterByCreatorIds.Contains(x.Creator));
                var customer = await _customerRepository.FindOneAsync(filter);

                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var result = _mapper.Map<GetShinhanDetailResponse>(customer);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetShinhanResponse>> GetAsync(GetShinhanResquest getShinhanResquest)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadShinhanManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadShinhanManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadShinhanManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadShinhanManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadShinhanManagement_ViewAll);

                var customerFilter = new CustonerFilterDto
                {
                    GreenType = GreenType.GreenE,
                    CreatorIds = filterByCreatorIds,
                    Status = getShinhanResquest.Status,
                    CustomerName = getShinhanResquest.CustomerName,
                    FromDate = getShinhanResquest.GetFromDate(),
                    ToDate = getShinhanResquest.GetToDate(),
                    ProductLine = getShinhanResquest.ProductLine,
                    ReturnStatus = getShinhanResquest.ReturnStatus,
                    PageIndex = getShinhanResquest.PageIndex,
                    PageSize = getShinhanResquest.PageSize,
                };
                var customers = await _customerRepository.GetAsync<GetShinhanResponse>(customerFilter);
                var total = await _customerRepository.CountAsync(customerFilter);

                var result = new PagingResponse<GetShinhanResponse>
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
                x.GreenType == GreenType.GreenE &&
                x.Personal.IdCard == idCard);
            return customerExisted != null;
        }
    }
}
