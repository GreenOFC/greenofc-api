using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IHistoryDomainService
    {
        Task CreateAsync(string referenceId, string referenceType, AuditActionType auditActionType, string actionName, object valueBefore = null, object valueAfter = null);
    }
    public class HistoryDomainService: IHistoryDomainService, IScopedLifetime
    {
        private readonly ILogger<HistoryDomainService> _logger;
        private readonly IUserLoginService _userLoginService;
        private readonly IMongoRepository<History> _historyRepository;
        private readonly IMongoRepository<HistoryV2> _historyV2Repository;

        public HistoryDomainService(
            ILogger<HistoryDomainService> logger,
            IUserLoginService userLoginService,
            IMongoRepository<History> historyRepository,
            IMongoRepository<HistoryV2> historyV2Repository)
        {
            _logger = logger;
            _userLoginService = userLoginService;
            _historyRepository = historyRepository;
            _historyV2Repository = historyV2Repository;
        }

        public async Task CreateAsync(string referenceId, string referenceType, AuditActionType auditActionType, string actionName, object valueBefore = null, object valueAfter = null)
        {
            await CreateHistoryAsync(referenceId, referenceType, auditActionType, actionName, valueBefore, valueAfter);
            // await CreateHistoryV2Async(referenceId, referenceType, auditActionType, actionName, valueBefore, valueAfter);
        }

        private async Task CreateHistoryAsync(string referenceId, string referenceType, AuditActionType auditActionType, string actionName, object valueBefore = null, object valueAfter = null)
        {
            try
            {
                var hisotry = new History(_userLoginService.GetUserId(), referenceId, referenceType, auditActionType, actionName, valueBefore, valueAfter);
                await _historyRepository.InsertOneAsync(hisotry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task CreateHistoryV2Async(string referenceId, string referenceType, AuditActionType auditActionType, string actionName, object valueBefore = null, object valueAfter = null)
        {
            try
            {
                var compare = ObjectHelpers.Compare(valueBefore, valueAfter);
                if(compare == null)
                {
                    return;
                }


                var hisotry = new HistoryV2(_userLoginService.GetUserId(), referenceId, referenceType, auditActionType, actionName, compare.OldValue, compare.NewValue);
                await _historyV2Repository.InsertOneAsync(hisotry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, referenceId);
                _logger.LogError(ex, JsonConvert.SerializeObject(valueBefore));
                _logger.LogError(ex, JsonConvert.SerializeObject(valueAfter));
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
