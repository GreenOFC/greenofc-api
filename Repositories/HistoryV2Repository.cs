using _24hplusdotnetcore.ModelDtos.History;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IHistoryV2Repository : IMongoRepository<HistoryV2>
    {
        Task<IEnumerable<GetHistoryV2Response>> GetAsync(IEnumerable<string> creatorIds, string customerId, int pageIndex, int pageSize);

        Task<long> CountAsync(IEnumerable<string> creatorIds, string textSearch);
    }
    public class HistoryV2Repository: MongoRepository<HistoryV2>, IHistoryV2Repository, IScopedLifetime
    {
        public HistoryV2Repository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetHistoryV2Response>> GetAsync(IEnumerable<string> creatorIds, string customerId, int pageIndex, int pageSize)
        {
            var filter = GetFilter(creatorIds, customerId);
            var projectMapping = new BsonDocument()
                {
                    { "ValueBefore", 1 },
                    { "ValueAfter", 1 },
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
                .As<GetHistoryV2Response>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(IEnumerable<string> creatorIds, string textSearch)
        {
            var filter = GetFilter(creatorIds, textSearch);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }


        private FilterDefinition<HistoryV2> GetFilter(IEnumerable<string> creatorIds, string customerId)
        {
            var filter = Builders<HistoryV2>.Filter.Empty;
            if (!string.IsNullOrEmpty(customerId))
            {
                filter &= Builders<HistoryV2>.Filter.Eq(x => x.ReferenceId, customerId);
            }
            if (creatorIds.Any())
            {
                filter &= Builders<HistoryV2>.Filter.In(x => x.Creator, creatorIds);
            }
            return filter;
        }
    }
}
