using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadHomes;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadHomeService
    {
        Task<PagingResponse<GetLeadHomeResponse>> GetAsync(GetLeadHomeRequest getLeadHomeRequest);
        Task CreateAsync(CreateLeadHomeRequest createLeadHomeRequest);
        Task UpdateAsync(string id, UpdateLeadHomeRequest updateLeadHomeRequest);
        Task<GetDetailLeadHomeResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
    }

    public class LeadHomeService: ILeadHomeService, IScopedLifetime
    {
        private readonly ILogger<LeadHomeService> _logger;
        private readonly ILeadHomeRepository _leadHomeRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadHomeService(
            ILogger<LeadHomeService> logger,
            ILeadHomeRepository leadHomeRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _leadHomeRepository = leadHomeRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _userService = userService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _posRepository = posRepository;
        }

        public async Task<PagingResponse<GetLeadHomeResponse>> GetAsync(GetLeadHomeRequest getLeadHomeRequest)
        {
            try
            {
                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                var filterCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadHomeManagement_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadHomeManagement_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadHomeManagement_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadHomeManagement_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadHomeManagement_ViewAll);

                var leadHomes = await _leadHomeRepository.GetAsync(filterCreatorIds, getLeadHomeRequest);
                var total = await _leadHomeRepository.CountAsync(filterCreatorIds, getLeadHomeRequest);

                return new PagingResponse<GetLeadHomeResponse>
                {
                    TotalRecord = total,
                    Data = leadHomes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateLeadHomeRequest createLeadHomeRequest)
        {
            try
            {
                var leadHome = _mapper.Map<LeadHome>(createLeadHomeRequest);

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                leadHome.TeamLeadInfo = user.TeamLeadInfo;
                leadHome.AsmInfo = user.AsmInfo;
                leadHome.PosInfo = user.PosInfo;
                leadHome.SaleInfomation = _mapper.Map<SaleInfomation>(user);
                leadHome.SaleChanelInfo = user.SaleChanelInfo;

                leadHome.Creator = _userLoginService.GetUserId();
                await _leadHomeRepository.InsertOneAsync(leadHome);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadSourceId = leadHome.Id,
                    LeadSource = LeadSourceType.LeadSource
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateLeadHomeRequest updateLeadHomeRequest)
        {
            try
            {
                var leadHome = await _leadHomeRepository.FindByIdAsync(id);

                if (leadHome == null)
                {
                    throw new ArgumentException(Common.Message.LEAD_VIB_NOT_FOUND);
                }

                _mapper.Map(updateLeadHomeRequest, leadHome);
                leadHome.Modifier = _userLoginService.GetUserId();
                leadHome.ModifiedDate = DateTime.Now;

                await _leadHomeRepository.ReplaceOneAsync(leadHome);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadSourceId = id,
                    LeadSource = LeadSourceType.LeadSource
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetDetailLeadHomeResponse> GetDetailAsync(string id)
        {
            try
            {
                var leadHome = await _leadHomeRepository.GetDetailAsync(id);

                if (leadHome == null)

                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(leadHome)));
                }

                return leadHome;
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
                var leadHome = await _leadHomeRepository.FindByIdAsync(id);

                if (leadHome == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(leadHome)));
                }

                leadHome.IsDeleted = true;
                leadHome.DeletedDate = DateTime.Now;
                leadHome.DeletedBy = _userLoginService.GetUserId();

                await _leadHomeRepository.ReplaceOneAsync(leadHome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
