using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.TransactionHistories;
using _24hplusdotnetcore.Repositories.Transaction;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Transaction
{
    public interface ITransactionHistoryService
    {
        Task<PagingResponse<TransactionHistoryResponse>> GetAsync(TransactionHistoryRequest request);
    }

    public class TransactionHistoryService : ITransactionHistoryService, IScopedLifetime
    {
        private readonly ILogger<TransactionHistoryService> _logger;
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;

        public TransactionHistoryService(
            ILogger<TransactionHistoryService> logger,
            ITransactionHistoryRepository transactionHistoryRepository)
        {
            _logger = logger;
            _transactionHistoryRepository = transactionHistoryRepository;
        }

        public async Task<PagingResponse<TransactionHistoryResponse>> GetAsync(TransactionHistoryRequest request)
        {
            try
            {
                var transactionHistories = await _transactionHistoryRepository.GetAsync(request);
                var total = await _transactionHistoryRepository.CountAsync(request);

                return new PagingResponse<TransactionHistoryResponse>
                {
                    TotalRecord = total,
                    Data = transactionHistories
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
