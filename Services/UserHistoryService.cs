using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.UserHistories;
using _24hplusdotnetcore.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IUserHistoryService
    {
        Task<PagingResponse<UserHistoryResponse>> GetAsync(UserHistoryRequest request);
    }

    public class UserHistoryService : IUserHistoryService, IScopedLifetime
    {
        private readonly ILogger<UserHistoryService> _logger;
        private readonly IUserHistoryRepository _userHistoryRepository;

        public UserHistoryService(
            ILogger<UserHistoryService> logger,
            IUserHistoryRepository userHistoryRepository)
        {
            _logger = logger;
            _userHistoryRepository = userHistoryRepository;
        }

        public async Task<PagingResponse<UserHistoryResponse>> GetAsync(UserHistoryRequest request)
        {
            try
            {
                var userHistories = await _userHistoryRepository.GetAsync(request);
                var total = await _userHistoryRepository.CountAsync(request);

                return new PagingResponse<UserHistoryResponse>
                {
                    TotalRecord = total,
                    Data = userHistories
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
