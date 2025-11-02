using _24hplusdotnetcore.ModelDtos.UserHistories;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IUserHistoryRepository : IMongoRepository<UserHistory>
    {
        Task<IEnumerable<UserHistoryResponse>> GetAsync(UserHistoryRequest userHistoryRequest);

        Task<long> CountAsync(UserHistoryRequest userHistoryRequest);
    }


    public class UserHistoryRepository : MongoRepository<UserHistory>, IUserHistoryRepository, IScopedLifetime
    {
        public UserHistoryRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<UserHistoryResponse>> GetAsync(UserHistoryRequest userHistoryRequest)
        {
            var filter = GetFilter(userHistoryRequest);

            return await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((userHistoryRequest.PageIndex - 1) * userHistoryRequest.PageSize)
                .Limit(userHistoryRequest.PageSize)
                .As<UserHistoryResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(UserHistoryRequest userHistoryRequest)
        {
            var filter = GetFilter(userHistoryRequest);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }


        private FilterDefinition<UserHistory> GetFilter(UserHistoryRequest userHistoryRequest)
        {
            var filter = Builders<UserHistory>.Filter.Ne(x => x.IsDeleted, true);

            filter &= Builders<UserHistory>.Filter.Gte(x => x.ModifiedDate, userHistoryRequest.GetFromDate());
            filter &= Builders<UserHistory>.Filter.Lte(x => x.ModifiedDate, userHistoryRequest.GetToDate());

            if (!string.IsNullOrEmpty(userHistoryRequest.UserId))
            {
                filter &= Builders<UserHistory>.Filter.Eq(x => x.Payload.Id, userHistoryRequest.UserId);
            }

            return filter;
        }
    }
}
