using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadVbis;
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
    public interface ILeadVbiService
    {
        Task<PagingResponse<GetLeadVbiResponse>> GetAsync(PagingRequest getLeadVbiRequest);
        Task CreateAsync(CreateLeadVbiRequest createLeadVbiRequest);
        Task UpdateAsync(string id, UpdateLeadVbiRequest updateLeadVbiRequest);
        Task<GetDetailLeadVbiResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
    }
    public class LeadVbiService: ILeadVbiService, IScopedLifetime
    {
        private readonly ILogger<LeadVbiService> _logger;
        private readonly ILeadVbiRepository _leadVbiRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadVbiService(
            ILogger<LeadVbiService> logger,
            ILeadVbiRepository leadVbiRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _leadVbiRepository = leadVbiRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _userService = userService;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _posRepository = posRepository;
        }

        public async Task<PagingResponse<GetLeadVbiResponse>> GetAsync(PagingRequest pagingRequest)
        {
            try
            {
                var filterCreators = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadVbiManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadVbiManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_LeadVbiManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_LeadVbiManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadVbiManagement_ViewAll);

                var leadVbis = await _leadVbiRepository.GetAsync(filterCreators, pagingRequest);
                var total = await _leadVbiRepository.CountAsync(filterCreators, pagingRequest);

                return new PagingResponse<GetLeadVbiResponse>
                {
                    TotalRecord = total,
                    Data = leadVbis
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateLeadVbiRequest createLeadVbiRequest)
        {
            try
            {
                var leadVbi = _mapper.Map<LeadVbi>(createLeadVbiRequest);

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                leadVbi.TeamLeadInfo = user.TeamLeadInfo;
                leadVbi.AsmInfo = user.AsmInfo;
                leadVbi.PosInfo = user.PosInfo;
                leadVbi.SaleChanelInfo = user.SaleChanelInfo;
                leadVbi.SaleInfomation = _mapper.Map<SaleInfomation>(user);

                leadVbi.Creator = _userLoginService.GetUserId();
                await _leadVbiRepository.InsertOneAsync(leadVbi);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadSourceId = leadVbi.Id,
                    LeadSource = LeadSourceType.LeadSource
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateLeadVbiRequest updateLeadVbiRequest)
        {
            try
            {
                var leadVbi = await _leadVbiRepository.FindByIdAsync(id);

                if (leadVbi == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(LeadVbi)));
                }

                _mapper.Map(updateLeadVbiRequest, leadVbi);
                leadVbi.Modifier = _userLoginService.GetUserId();
                leadVbi.ModifiedDate = DateTime.Now;

                await _leadVbiRepository.ReplaceOneAsync(leadVbi);

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

        public async Task<GetDetailLeadVbiResponse> GetDetailAsync(string id)
        {
            try
            {
                var leadVbi = await _leadVbiRepository.GetDetailAsync(id);

                if (leadVbi == null)

                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(LeadVbi)));
                }

                return leadVbi;
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
                var leadVbi = await _leadVbiRepository.FindByIdAsync(id);

                if (leadVbi == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(LeadVbi)));
                }

                leadVbi.IsDeleted = true;
                leadVbi.DeletedDate = DateTime.Now;
                leadVbi.DeletedBy = _userLoginService.GetUserId();

                await _leadVbiRepository.ReplaceOneAsync(leadVbi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
