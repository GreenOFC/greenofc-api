using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.eWalletTransaction
{
    public interface ITransactionLogRepository: IMongoRepository<TransactionLogModel>
    {

        Task<TransactionLogModel> Insert(TransactionLogModel eWalletTransactionMaster);
        Task<IEnumerable<TransactionLogModel>> GetListTransactionLogById(string TransactionId);
        IEnumerable<TransactionLogModel> GetListTransactionLog();
    }
    public class TransactionLogRepository : MongoRepository<TransactionLogModel>, ITransactionLogRepository, IScopedLifetime
    {
        private readonly ILogger<TransactionLogRepository> _logger;
        public TransactionLogRepository(IMongoDbConnection connection, ILogger<TransactionLogRepository> logger) : base(connection)
        {
            _logger = logger;
        }

        public IEnumerable<TransactionLogModel> GetListTransactionLog()
        {
            try
            {
                return base.AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<TransactionLogModel>> GetListTransactionLogById(string TransactionId)
        {
            try
            {
                var walletTransactionLog = await FilterByAsync(x => x.PartnerTransaction == TransactionId);
                return walletTransactionLog.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<TransactionLogModel> Insert(TransactionLogModel walletTransactionLog)
        {
            try
            {
                string num = GetRandomHexNumber(24);
                walletTransactionLog.Id = num;
                await InsertOneAsync(walletTransactionLog);
                var walletLog = await GetTransactionLogById(walletTransactionLog.Id);
                return walletLog;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<TransactionLogModel> GetTransactionLogById(string Id)
        {
            try
            {
                return await FindOneAsync(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private static string GetRandomHexNumber(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

    }
}
