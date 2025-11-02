using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.F88;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.F88
{
    public interface IF88Service
    {
        Task<PagingResponse<GetF88Response>> GetAsync(GetF88Request getF88Request);
        Task<CreateF88Response> CreateAsync(CreateF88Request createF88Request);
        Task UpdateAsync(string id, UpdateF88Request updateF88Request);
        Task<GetF88DetailResponse> GetDetailAsync(string id);
        Task<GetF88LinkResponse> GetLinkAsync(string id);
    }

    public class F88Service : IF88Service, IScopedLifetime
    {
        private readonly ILogger<F88Service> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IMongoRepository<POS> _posRepository;
        private readonly IUserServices _userServices;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly F88Config _f88Config;
        private readonly IRestF88AuthenService _restF88AuthenService;
        private readonly IMemoryCache _memoryCache;

        public F88Service(
            ILogger<F88Service> logger,
            ICustomerRepository customerRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IMongoRepository<POS> posRepository,
            IUserServices userServices,
            IHistoryDomainService historyDomainService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IOptions<F88Config> f88ConfigOptions,
            IRestF88AuthenService restF88AuthenService,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _posRepository = posRepository;
            _userServices = userServices;
            _historyDomainService = historyDomainService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _f88Config = f88ConfigOptions.Value;
            _restF88AuthenService = restF88AuthenService;
            _memoryCache = memoryCache;
        }

        public async Task<PagingResponse<GetF88Response>> GetAsync(GetF88Request getF88Request)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadF88Management_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadF88Management_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadF88Management_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadF88Management_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadF88Management_ViewAll);

                var customerFilter = new CustonerFilterDto
                {
                    CustomerName = getF88Request.TextSearch,
                    Sale = getF88Request.Sale,
                    PosManager = getF88Request.PosManager,
                    TeamLead = getF88Request.TeamLead,
                    GreenType = GreenType.GreenF88,
                    CreatorIds = filterByCreatorIds,
                    Status = getF88Request.Status,
                    FromDate = getF88Request.GetFromDate(),
                    ToDate = getF88Request.GetToDate(),
                    PageIndex = getF88Request.PageIndex,
                    PageSize = getF88Request.PageSize
                };

                var customers = await _customerRepository.GetAsync<GetF88Response>(customerFilter);

                var total = await _customerRepository.CountAsync(customerFilter);

                var result = new PagingResponse<GetF88Response>
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

        public async Task<CreateF88Response> CreateAsync(CreateF88Request createF88Request)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createF88Request);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.Creator = _userLoginService.GetUserId();
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    LeadSource = LeadSourceType.F88
                });

                var response = _mapper.Map<CreateF88Response>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateF88Request updateF88Request)
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

                var valueBefore = customer.Clone();

                _mapper.Map(updateF88Request, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateAsync), valueBefore, customer);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    LeadSource = LeadSourceType.F88
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetF88DetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadF88Management_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadF88Management_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadF88Management_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadF88Management_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadF88Management_ViewAll);

                Expression<Func<Customer, bool>> filter = x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    (!filterByCreatorIds.Any() || filterByCreatorIds.Contains(x.Creator));
                var customer = await _customerRepository.FindOneAsync(filter);

                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var result = _mapper.Map<GetF88DetailResponse>(customer);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetF88LinkResponse> GetLinkAsync(string id)
        {
            try
            {
                string accessToken = string.Empty;

                // Key not in cache, so get data.
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var user = await _userRepository.FindByIdAsync(customer.Creator);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                string cacheKey = $"f88:accessToken:{id}";
                if (!_memoryCache.TryGetValue(cacheKey, out accessToken))
                {
                    var authenRequest = new F88LoginRestRequest { UserName = _f88Config.UserName, Password = _f88Config.Password, IncludeRoles = _f88Config.IncludeRoles };
                    var authen = await _restF88AuthenService.GetAuthAsync(authenRequest);

                    accessToken = authen.AccessToken;

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(authen.ExpiresIn));

                    // Save data in cache.
                    _memoryCache.Set(cacheKey, accessToken, cacheEntryOptions);
                }

                return new GetF88LinkResponse
                {
                    Link = string.Format(_f88Config.UrlRedirect, user.UserName, user.FullName, accessToken)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
