using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.CheckCustomers;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ICheckCustomerDetailRepository : IMongoRepository<CheckCustomerDetail>
    {
        Task<IEnumerable<CheckCustomerDetailResponse>> GetAsync(string textSearch, string fileId, int pageIndex = 1, int pageSize = 10);

        Task<long> CountAsync(string textSearch, string fileId);
    }

    public class CheckCustomerDetailRepository : MongoRepository<CheckCustomerDetail>, ICheckCustomerDetailRepository, IScopedLifetime
    {
        public CheckCustomerDetailRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<CheckCustomerDetailResponse>> GetAsync(string textSearch, string fileId, int pageIndex = 1, int pageSize = 10)
        {
            var filter = GetFilter(textSearch, fileId);

            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .As<CheckCustomerDetailResponse>()
                    .ToListAsync();

            return result;
        }

        public async Task<long> CountAsync(string textSearch, string fileId)
        {
            var filter = GetFilter(textSearch, fileId);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<CheckCustomerDetail> GetFilter(string textSearch, string fileId)
        {
            var filter = Builders<CheckCustomerDetail>.Filter.Eq(x => x.IsDeleted, false);

            if (!string.IsNullOrEmpty(textSearch))
            {
                var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<CheckCustomerDetail>.Filter.Regex(x => x.IdCard, regex);
            }

            if (!string.IsNullOrEmpty(fileId))
            {
                filter &= Builders<CheckCustomerDetail>.Filter.Eq(x => x.FileId, fileId);
            }

            return filter;
        }

    }
}
