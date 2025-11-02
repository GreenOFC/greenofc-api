using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ISaleChanelConfigUserRepository : IMongoRepository<SaleChanelConfigUser>
    {
        Task<IEnumerable<SaleChanelConfigUserResponse>> GetAsync(string textSearch, int pageIndex, int pageSize);
        Task<long> CountAsync(string textSearch);
        Task<SaleChanelConfigUserDetailResponse> GetDetailAsync(string id);
    }

    public class SaleChanelConfigUserRepository : MongoRepository<SaleChanelConfigUser>, ISaleChanelConfigUserRepository, IScopedLifetime
    {
        public SaleChanelConfigUserRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<SaleChanelConfigUserResponse>> GetAsync(string textSearch, int pageIndex, int pageSize)
        {
            var filter = GetFilter(textSearch);

            return await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .As<SaleChanelConfigUserResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(string textSearch)
        {
            var filter = GetFilter(textSearch);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        public async Task<SaleChanelConfigUserDetailResponse> GetDetailAsync(string id)
        {
            return await _collection
                .Aggregate()
                .Match(x => x.Id == id && x.IsDeleted != true)
                .As<SaleChanelConfigUserDetailResponse>()
                .FirstOrDefaultAsync();
        }


        private FilterDefinition<SaleChanelConfigUser> GetFilter(string textSearch)
        {
            var filter = Builders<SaleChanelConfigUser>.Filter.Ne(x => x.IsDeleted, true);

            if (!string.IsNullOrEmpty(textSearch))
            {
                var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<SaleChanelConfigUser>.Filter.Regex(x => x.SaleInfo.UserName, regex);
            }

            return filter;
        }
    }
}
