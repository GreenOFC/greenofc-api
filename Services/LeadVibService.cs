using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadVibs;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadVibService
    {
        Task<PagingResponse<GetLeadVibResponse>> GetAsync(GetLeadVibRequest getLeadVibRequest);
        Task<long> CountLeadAsync();
        Task CreateAsync(CreateLeadVibRequest createLeadVibRequest);
        Task UpdateAsync(string id, UpdateLeadVibRequest updateLeadVibRequest);
        Task<GetLeadVibResponse> GetDetailAsync(string id);
        Task<bool> CheckExistedLeadAsync(string phone);
        Task DeleteAsync(string id);
    }

    public class LeadVibService : ILeadVibService, IScopedLifetime
    {
        private readonly ILogger<LeadVibService> _logger;
        private readonly ILeadVibRepository _leadVibRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadVibService(
            ILogger<LeadVibService> logger,
            ILeadVibRepository leadVibRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _leadVibRepository = leadVibRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _userService = userService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _posRepository = posRepository;
        }

        public async Task<PagingResponse<GetLeadVibResponse>> GetAsync(GetLeadVibRequest getLeadVibRequest)
        {
            try
            {
                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                var filterCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadVibsManagement_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadVibsManagement_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadVibsManagement_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadVibsManagement_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadVibsManagement_ViewAll);
                var leadVibs = await _leadVibRepository.GetAsync(filterCreatorIds, getLeadVibRequest);
                var total = await _leadVibRepository.CountAsync(filterCreatorIds, getLeadVibRequest);
                var leadVibDtos = _mapper.Map<IEnumerable<GetLeadVibResponse>>(leadVibs);

                return new PagingResponse<GetLeadVibResponse>
                {
                    TotalRecord = total,
                    Data = leadVibDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<long> CountLeadAsync()
        {
            try
            {

                var filterCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadVibsManagement_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadVibsManagement_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadVibsManagement_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadVibsManagement_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadVibsManagement_ViewAll);
                return await _leadVibRepository.CountAsync(filterCreatorIds, new GetLeadVibRequest { TextSearch = string.Empty });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateLeadVibRequest createLeadVibRequest)
        {
            try
            {
                var leadVib = _mapper.Map<LeadVib>(createLeadVibRequest);

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                leadVib.TeamLeadInfo = user.TeamLeadInfo;
                leadVib.AsmInfo = user.AsmInfo;
                leadVib.PosInfo = user.PosInfo;
                leadVib.SaleChanelInfo = user.SaleChanelInfo;
                leadVib.SaleInfomation = _mapper.Map<SaleInfomation>(user);

                leadVib.Creator = _userLoginService.GetUserId();
                await _leadVibRepository.Create(leadVib);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadSourceId = leadVib.Id,
                    LeadSource = LeadSourceType.LeadSource
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateLeadVibRequest updateLeadVibRequest)
        {
            try
            {
                var leadVib = await _leadVibRepository.GetDetailAsync(id);

                if (leadVib == null)
                {
                    throw new ArgumentException(Common.Message.LEAD_VIB_NOT_FOUND);
                }

                _mapper.Map(updateLeadVibRequest, leadVib);

                leadVib.Id = id;
                leadVib.Modifier = _userLoginService.GetUserId();
                leadVib.ModifiedDate = DateTime.Now;

                await _leadVibRepository.ReplaceOneAsync(leadVib);

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

        public async Task<GetLeadVibResponse> GetDetailAsync(string id)
        {
            try
            {
                var leadVib = await _leadVibRepository.GetRelatedDetailAsync(id);

                if (leadVib == null)

                {
                    throw new ArgumentException(Common.Message.LEAD_VIB_NOT_FOUND);
                }

                return leadVib;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckExistedLeadAsync(string phone)
        {
            try
            {
                var leadVib = await _leadVibRepository.IsExist(phone);

                return leadVib;
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
                var leadVib = await _leadVibRepository.GetDetailAsync(id);

                if (leadVib == null)
                {
                    throw new ArgumentException(Common.Message.LEAD_VIB_NOT_FOUND);
                }

                leadVib.IsDeleted = true;
                leadVib.DeletedDate = DateTime.Now;
                leadVib.DeletedBy = _userLoginService.GetUserId();

                await _leadVibRepository.ReplaceOneAsync(leadVib);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
