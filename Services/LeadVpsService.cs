using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Vps;
using _24hplusdotnetcore.ModelResponses.VPS;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.VPS;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.VPS;
using _24hplusdotnetcore.Services.CRM;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadVpsService
    {
        Task CreateAsync(CreateLeadVpsDto request);
        Task UpdateAsync(string id, UpdateLeadVpsDto request);
        Task<LeadVpsDetailResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
        Task<PagingResponse<LeadVps>> GetList(PagingRequest pagingRequest);
    }

    public class LeadVpsService : ILeadVpsService, IScopedLifetime
    {
        private readonly ILogger<LeadVpsService> _logger;
        private readonly IMapper _mapper;
        private readonly ILeadVpsRepository _leadVpsRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadVpsService(ILogger<
            LeadVpsService> logger,
            IMapper mapper,
            ILeadVpsRepository leadVpsRepository,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _leadVpsRepository = leadVpsRepository;
            _userRepository = userRepository;
            _userLoginService = userLoginService;
            _userService = userService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _posRepository = posRepository;
        }

        public async Task CreateAsync(CreateLeadVpsDto request)
        {
            try
            {
                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                var leadVps = _mapper.Map<LeadVps>(request);

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                leadVps.TeamLeadInfo = user.TeamLeadInfo;
                leadVps.AsmInfo = user.AsmInfo;
                leadVps.PosInfo = user.PosInfo;
                leadVps.SaleInfomation = _mapper.Map<SaleInfomation>(user);

                leadVps.Creator = currentUser.Id;
                await _leadVpsRepository.Create(leadVps);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadSourceId = leadVps.Id,
                    LeadSource = LeadSourceType.LeadSource
                });
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
                await _leadVpsRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadVpsDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var result = await _leadVpsRepository.GetDetailAsync(id);
                var response = _mapper.Map<LeadVpsDetailResponse>(result);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<LeadVps>> GetList(PagingRequest pagingRequest)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadVpsManagement_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadVpsManagement_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadVpsManagement_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadVpsManagement_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadVpsManagement_ViewAll);

                var result = await _leadVpsRepository.GetList(filterByCreatorIds, pagingRequest);
                var rowCount = await _leadVpsRepository.CountLead(filterByCreatorIds, pagingRequest);

                return new PagingResponse<LeadVps>
                {
                    TotalRecord = rowCount,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateLeadVpsDto request)
        {
            try
            {
                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                var update = _mapper.Map<LeadVps>(request);
                update.Modifier = currentUser.Id;
                update.Id = id;
                await _leadVpsRepository.Update(update);

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
    }
}
