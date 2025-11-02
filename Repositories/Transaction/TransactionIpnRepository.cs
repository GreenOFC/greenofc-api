using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.Transaction;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.eWalletTransaction
{
    public interface ITransactionIpnRepository : IMongoRepository<TransactionIpnModel>
    {
        Task<IEnumerable<TransactionIpnResponse>> GetAsync(string transaction , string partnerTransaction, int pageIndex, int pageSize);

        Task<long> CountAsync(string transaction, string partnerTransaction);
    }
    public class TransactionIpnRepository : MongoRepository<TransactionIpnModel>, ITransactionIpnRepository, IScopedLifetime
    {
        public TransactionIpnRepository(
            IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<long> CountAsync(string transaction, string partnerTransaction)
        {
            var filter = GetFilter(transaction, partnerTransaction);

            var total = await _collection
                .Find(filter)
                .CountDocumentsAsync();

            return total;
        }

        public async Task<IEnumerable<TransactionIpnResponse>> GetAsync(string transaction, string partnerTransaction, int pageIndex, int pageSize)
        {
            var filter = GetFilter(transaction, partnerTransaction);
            var TransactionLst = await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(x => x.ModifiedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .As<TransactionIpnResponse>().ToListAsync();
            return TransactionLst;
        }

        private FilterDefinition<TransactionIpnModel> GetFilter(string transaction, string partnerTransaction)
        {
            var filter = Builders<TransactionIpnModel>.Filter.Empty;
            if (!string.IsNullOrEmpty(transaction))
            {
                var regex = new BsonRegularExpression($"/{transaction.ConvertSpecialCharacters()}/i");
                filter &= Builders<TransactionIpnModel>.Filter.Regex(x => x.Transaction, regex);
            }
            if (!string.IsNullOrEmpty(partnerTransaction))
            {
                var regex = new BsonRegularExpression($"/{partnerTransaction.ConvertSpecialCharacters()}/i");
                filter &= Builders<TransactionIpnModel>.Filter.Regex(x => x.PartnerTransaction, regex);
            }
            return filter;
        }
    }
}
