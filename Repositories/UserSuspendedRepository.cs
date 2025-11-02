using _24hplusdotnetcore.ModelDtos.UserSuspendeds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IUserSuspendedRepository : IMongoRepository<UserSuspended>
    {
        Task<IEnumerable<GetUserSuspendedResponse>> GetAsync(int pageIndex, int pageSize);
        Task<long> CountAsync();
        Task<GetDetailUserSuspendedResponse> GetDetailAsync(string id);
    }

    public class UserSuspendedRepository : MongoRepository<UserSuspended>, IUserSuspendedRepository, IScopedLifetime
    {
        public UserSuspendedRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetUserSuspendedResponse>> GetAsync(int pageIndex, int pageSize)
        {
            var filter = GetFilter();
            return await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(x => x.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .As<GetUserSuspendedResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync()
        {
            var filter = GetFilter();
            var total = await _collection.Find(filter).CountDocumentsAsync();

            return total;
        }

        public async Task<GetDetailUserSuspendedResponse> GetDetailAsync(string id)
        {
            return await _collection
                .Aggregate()
                .Match(x => !x.IsDeleted && x.Id == id)
                .As<GetDetailUserSuspendedResponse>()
                .FirstOrDefaultAsync();
        }

        private FilterDefinition<UserSuspended> GetFilter()
        {
            return Builders<UserSuspended>.Filter.Ne(x => x.IsDeleted, true);
        }
    }
}
