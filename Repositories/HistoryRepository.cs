using _24hplusdotnetcore.ModelDtos.History;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IHistoryRepository : IMongoRepository<History>
    {
        Task<IEnumerable<GetHistoryResponse>> GetAsync(string customerId, int pageIndex, int pageSize);
        
        Task<long> CountAsync(string textSearch);
    }
    public class HistoryRepository : MongoRepository<History>, IHistoryRepository, IScopedLifetime
    {
        public HistoryRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetHistoryResponse>> GetAsync(string customerId, int pageIndex, int pageSize)
        {
            var filter = GetFilter(customerId);
            var projectMapping = new BsonDocument()
                {
                    { "ValueBefore", 1 },
                    { "ValueAfter", 1 },
                    { "OldValue", 1 },
                    { "NewValue", 1 },
                    { "CreatedDate", 1 },
                    { "SaleInfo._id", 1},
                    { "SaleInfo.FullName", 1},
                    { "SaleInfo.UserName", 1}
                };
            return await _collection.Aggregate()
                .Match(filter)
                .SortByDescending(c => c.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .Lookup("Users", "Creator", "_id", "SaleInfo")
                .Project(projectMapping)
                .As<GetHistoryResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(string textSearch)
        {
            var filter = GetFilter(textSearch);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }


        private FilterDefinition<History> GetFilter(string customerId)
        {
            var filter = Builders<History>.Filter.Empty;
            if (!string.IsNullOrEmpty(customerId))
            {
                filter &= Builders<History>.Filter.Eq(x => x.ReferenceId, customerId);
            }
            return filter;
        }
    }
}
