using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.History;
using _24hplusdotnetcore.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IHistoryService
    {
        Task<PagingResponse<GetHistoryResponse>> GetAsync(GetHistoryRequest request);
    }

    public class HistoryService : IHistoryService, IScopedLifetime
    {
        private readonly ILogger<LeadVibService> _logger;
        private readonly IHistoryRepository _historyRepository;

        public HistoryService(
            ILogger<LeadVibService> logger,
            IHistoryRepository historyRepository)
        {
            _logger = logger;
            _historyRepository = historyRepository;
        }

        public async Task<PagingResponse<GetHistoryResponse>> GetAsync(GetHistoryRequest request)
        {
            try
            {
                var history = await _historyRepository.GetAsync(request.CustomerId, request.PageIndex, request.PageSize);
                var total = await _historyRepository.CountAsync(request.CustomerId);

                return new PagingResponse<GetHistoryResponse>
                {
                    TotalRecord = total,
                    Data = history
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
