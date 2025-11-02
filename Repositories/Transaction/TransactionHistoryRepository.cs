using _24hplusdotnetcore.ModelDtos.TransactionHistories;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.Transaction;
using _24hplusdotnetcore.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.Transaction
{
    public interface ITransactionHistoryRepository: IMongoRepository<TransactionHistory>
    {
        Task<IEnumerable<TransactionHistoryResponse>> GetAsync(TransactionHistoryRequest transactionHistoryRequest);

        Task<long> CountAsync(TransactionHistoryRequest transactionHistoryRequest);
    }

    public class TransactionHistoryRepository: MongoRepository<TransactionHistory>, ITransactionHistoryRepository, IScopedLifetime
    {
        public TransactionHistoryRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<TransactionHistoryResponse>> GetAsync(TransactionHistoryRequest transactionHistoryRequest)
        {
            var filter = GetFilter(transactionHistoryRequest);

            return await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((transactionHistoryRequest.PageIndex - 1) * transactionHistoryRequest.PageSize)
                .Limit(transactionHistoryRequest.PageSize)
                .As<TransactionHistoryResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(TransactionHistoryRequest transactionHistoryRequest)
        {
            var filter = GetFilter(transactionHistoryRequest);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }


        private FilterDefinition<TransactionHistory> GetFilter(TransactionHistoryRequest transactionHistoryRequest)
        {
            var filter = Builders<TransactionHistory>.Filter.Ne(x => x.IsDeleted, true);
            
            filter &= Builders<TransactionHistory>.Filter.Gte(x => x.ModifiedDate, transactionHistoryRequest.GetFromDate());
            filter &= Builders<TransactionHistory>.Filter.Lte(x => x.ModifiedDate, transactionHistoryRequest.GetToDate());
            
            if (!string.IsNullOrEmpty(transactionHistoryRequest.TransactionId))
            {
                filter &= Builders<TransactionHistory>.Filter.Eq(x => x.Payload.Id, transactionHistoryRequest.TransactionId);
            }

            return filter;
        }
    }
}
