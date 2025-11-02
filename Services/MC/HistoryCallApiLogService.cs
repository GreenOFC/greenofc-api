using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.MC;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public interface IHistoryCallApiLogService
    {
        Task<PagingResponse<HistoryCallApiLogResponse>> GetAsync(HistoryCallApiLogRequest request);
    }
    public class HistoryCallApiLogService : IHistoryCallApiLogService, IScopedLifetime
    {
        private readonly ILogger<HistoryCallApiLogService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IHistoryCallApiLogRepository _historyCallApiLogRepository;
        private readonly IUserServices _userServices;

        public HistoryCallApiLogService(
            ILogger<HistoryCallApiLogService> logger,
            IMapper mapper,
            IOptions<MCConfig> mCConfigOptions,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userServices,
            IHistoryCallApiLogRepository historyCallApiLogRepository
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _userServices = userServices;
            _historyCallApiLogRepository = historyCallApiLogRepository;
        }


        public async Task<PagingResponse<HistoryCallApiLogResponse>> GetAsync(HistoryCallApiLogRequest request)
        {
            try
            {

                var filterCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.LogApi_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.LogApi_ViewAll,
                    PermissionCost.PosLeadPermission.LogApi_ViewAll,
                    PermissionCost.AsmPermission.LogApi_ViewAll,
                    PermissionCost.TeamLeaderPermission.LogApi_ViewAll);

                var data = await _historyCallApiLogRepository.GetAsync(request, filterCreatorIds);
                var total = await _historyCallApiLogRepository.CountAsync(request, filterCreatorIds);

                return new PagingResponse<HistoryCallApiLogResponse>
                {
                    TotalRecord = total,
                    Data = data
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
