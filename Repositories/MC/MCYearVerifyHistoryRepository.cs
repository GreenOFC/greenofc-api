using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.MC
{
    public interface IMCYearVerifyHistoryRepository
    {
        Task Create(MCYearVerifyHistory request);
    }

    public class MCYearVerifyHistoryRepository : IMCYearVerifyHistoryRepository, IScopedLifetime
    {
        private readonly ILogger<MCYearVerifyHistoryRepository> _logger;
        private readonly IMongoRepository<MCYearVerifyHistory> _mcYearVerifyHistory;

        public MCYearVerifyHistoryRepository(ILogger<MCYearVerifyHistoryRepository> logger, IMongoRepository<MCYearVerifyHistory> mcYearVerifyHistory)
        {
            this._logger = logger;
            this._mcYearVerifyHistory = mcYearVerifyHistory;
        }

        public async Task Create(MCYearVerifyHistory request)
        {
            try
            {
                await _mcYearVerifyHistory.InsertOneAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
