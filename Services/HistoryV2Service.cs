using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.History;
using _24hplusdotnetcore.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IHistoryV2Service
    {
        Task<PagingResponse<GetHistoryV2Response>> GetAsync(GetHistoryRequest request);
    }
    public class HistoryV2Service: IHistoryV2Service, IScopedLifetime
    {
        private readonly ILogger<HistoryV2Service> _logger;
        private readonly IHistoryV2Repository _historyV2Repository;
        private readonly IUserServices _userServices;

        public HistoryV2Service(
            ILogger<HistoryV2Service> logger,
            IHistoryV2Repository historyV2Repository,
            IUserServices userServices)
        {
            _logger = logger;
            _historyV2Repository = historyV2Repository;
            _userServices = userServices;
        }

        public async Task<PagingResponse<GetHistoryV2Response>> GetAsync(GetHistoryRequest request)
        {
            try
            {
                var filterByCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_HistoryManagement_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_HistoryManagement_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_HistoryManagement_ViewAll,
                    PermissionCost.AsmPermission.Asm_HistoryManagement_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_HistoryManagement_ViewAll);

                var histories = await _historyV2Repository.GetAsync(filterByCreatorIds, request.CustomerId, request.PageIndex, request.PageSize);
                var total = await _historyV2Repository.CountAsync(filterByCreatorIds, request.CustomerId);

                return new PagingResponse<GetHistoryV2Response>
                {
                    TotalRecord = total,
                    Data = histories
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
