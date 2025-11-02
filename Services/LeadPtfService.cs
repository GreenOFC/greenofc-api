using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadPtf;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Services.PtfOmnis;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadPtfService
    {
        Task<PagingResponse<GetLeadPtfResponse>> GetAsync(GetLeadPtfRequest pagingRequest);
        Task<GetDetailLeadPtfResponse> GetDetailAsync(string id);
        Task<CreateLeadPtfResponse> CreateAsync<T>(T createLeadPtfRequest) where T : ICreateLeadPtf;
        Task UpdateAsync<T>(string id, T payload) where T : IUpdateLeadPtf;
        Task<LeadPtfCategoryGroupDto> GetCategoryAsync(string productLine);
    }

    public class LeadPtfService : ILeadPtfService, IScopedLifetime
    {
        private readonly ILogger<LeadPtfService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        private readonly IMongoRepository<POS> _posRepository;
        private readonly IMongoRepository<ChecklistModel> _checklistModelRepository;
        private readonly IMongoRepository<LeadPtfCategoryGroup> _leadPtfCategoryGroupRepository;
        private readonly IPtfOmniDataProcessingService _ptfOmniDataProcessingService;

        public LeadPtfService(
            ILogger<LeadPtfService> logger,
            IMapper mapper,
            IUserLoginService userLoginService,
            ICustomerRepository customerRepository,
            IHistoryDomainService historyDomainService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IUserRepository userRepository,
            IUserServices userService,
            IMongoRepository<POS> posRepository,
            IMongoRepository<ChecklistModel> checklistModelRepository,
            IMongoRepository<LeadPtfCategoryGroup> leadPtfCategoryGroupRepository,
            IPtfOmniDataProcessingService ptfOmniDataProcessingService)
        {
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _customerRepository = customerRepository;
            _historyDomainService = historyDomainService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _userRepository = userRepository;
            _userService = userService;
            _posRepository = posRepository;
            _checklistModelRepository = checklistModelRepository;
            _leadPtfCategoryGroupRepository = leadPtfCategoryGroupRepository;
            _ptfOmniDataProcessingService = ptfOmniDataProcessingService;
        }

        public async Task<PagingResponse<GetLeadPtfResponse>> GetAsync(GetLeadPtfRequest pagingRequest)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadPtfManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadPtfManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadPtfManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadPtfManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadPtfManagement_ViewAll);

                var customerFilter = new CustonerFilterDto
                {
                    GreenType = GreenType.GreenP,
                    ProductLine = pagingRequest.ProductLine,
                    CreatorIds = filterByCreatorIds,
                    FromCreateDate = pagingRequest.GetFromDate(),
                    ToCreateDate = pagingRequest.GetToDate(),
                    CustomerName = pagingRequest.CustomerName,
                    PosManager = pagingRequest.PosManager,
                    TeamLead = pagingRequest.TeamLead,
                    Asm = pagingRequest.Asm,
                    Sale = pagingRequest.Sale,
                    ReturnStatus = pagingRequest.ReturnStatus,
                    Status = pagingRequest.Status,
                    PageIndex = pagingRequest.PageIndex,
                    PageSize = pagingRequest.PageSize
                };

                var customers = await _customerRepository.GetAsync<GetLeadPtfResponse>(customerFilter);

                var total = await _customerRepository.CountAsync(customerFilter);

                var result = new PagingResponse<GetLeadPtfResponse>
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

        public async Task<GetDetailLeadPtfResponse> GetDetailAsync(string id)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadPtfManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadPtfManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadPtfManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadPtfManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadPtfManagement_ViewAll);

                Expression<Func<Customer, bool>> filter = x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                     x.GreenType == GreenType.GreenP &&
                    (!filterByCreatorIds.Any() || filterByCreatorIds.Contains(x.Creator));
                var customer = await _customerRepository.FindOneAsync(filter);

                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var result = _mapper.Map<GetDetailLeadPtfResponse>(customer);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CreateLeadPtfResponse> CreateAsync<T>(T createLeadPtfRequest) where T: ICreateLeadPtf
        {
            try
            {
                var hasExiested = await IsExistedAsync(createLeadPtfRequest.Personal.IdCard, createLeadPtfRequest.Personal.Phone);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                var checkList = await _checklistModelRepository.FindOneAsync(x =>
                    x.GreenType == GreenType.GreenP && x.ProductLine == createLeadPtfRequest.ProductLine);

                var customer = _mapper.Map<Customer>(createLeadPtfRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.Documents = checkList?.Checklist;
                customer.UpdateMandatoryDocument();
                customer.Creator = _userLoginService.GetUserId();
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                var response = _mapper.Map<CreateLeadPtfResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync<T>(string id, T payload) where T: IUpdateLeadPtf
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                if(payload is IUpdateLeadPtfPersonal updateLeadPtfPersonal)
                {
                    var hasExiested = (customer.Personal.IdCard != updateLeadPtfPersonal.Personal.IdCard || customer.Personal.Phone != updateLeadPtfPersonal.Personal.Phone) &&
                    await IsExistedAsync(updateLeadPtfPersonal.Personal.IdCard, updateLeadPtfPersonal.Personal.Phone, id);
                    if (hasExiested)
                    {
                        throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                    }
                }

                var valueBefore = customer.Clone();

                _mapper.Map(payload, customer);

                customer.UpdateMandatoryDocument();
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateAsync), valueBefore, customer);

                if (payload is ISubmitLeadPtf submitLeadPtf && submitLeadPtf.IsSubmit)
                {
                    await SubmitAsync(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadPtfCategoryGroupDto> GetCategoryAsync(string productLine)
        {
            var categoryGroup = await _leadPtfCategoryGroupRepository.FindOneAsync(x => x.ProductLine == productLine);
            return _mapper.Map<LeadPtfCategoryGroupDto>(categoryGroup);
        }

        private async Task SubmitAsync(Customer customer)
        {
            var validator = new LeadPtfValidation();
            var result = validator.Validate(customer);
            var valueBefore = customer.Clone();

            if (!result.IsValid)
            {
                customer.Status = CustomerStatus.RETURN;
                customer.Result.Reason = Message.VALIDATTE_EERROR_REQUIRED + string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitAsync), valueBefore, customer);

                throw new ArgumentException(string.Format(Message.COMMON_REQUIRED, customer.Result.Reason));
            }

            customer.Status = CustomerStatus.SENDING;
            customer.Modifier = _userLoginService.GetUserId();
            customer.ModifiedDate = DateTime.Now;
            await _customerRepository.ReplaceOneAsync(customer);

            await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitAsync), valueBefore, customer);

            _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
            {
                CustomerId = customer.Id,
                LeadSource = LeadSourceType.PTF,
            });
            await _ptfOmniDataProcessingService.CreateOneAsync(customer);
            BackgroundJob.Enqueue<IPtfOmniDataProcessingService>(x => x.SyncDataAsync(customer.Id));
        }

        private async Task<bool> IsExistedAsync(string idCard, string phone, string id = null)
        {
            return await _customerRepository.GetAnyAsync(x =>
                !x.IsDeleted &&
                x.Id != id &&
                x.GreenType == GreenType.GreenP &&
                x.Personal.IdCard == idCard &&
                x.Personal.Phone == phone &&
                (x.Status == CustomerStatus.SUBMIT && x.Status == CustomerStatus.PROCESSING && x.Status == CustomerStatus.RETURN));
        }
    }
}
