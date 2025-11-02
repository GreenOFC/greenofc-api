using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.CheckCustomers;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ICheckCustomerRepository : IMongoRepository<CheckCustomer>
    {
        Task<IEnumerable<CheckCustomerResponse>> GetAsync(string textSearch, IEnumerable<string> creatorIds, int pageIndex = 1, int pageSize = 10);

        Task<long> CountAsync(string textSearch, IEnumerable<string> creatorIds);
    }
    public class CheckCustomerRepository : MongoRepository<CheckCustomer>, IScopedLifetime, ICheckCustomerRepository
    {
        public CheckCustomerRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<CheckCustomerResponse>> GetAsync(string textSearch, IEnumerable<string> creatorIds, int pageIndex = 1, int pageSize = 10)
        {
            var filter = GetFilter(textSearch, creatorIds);
            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .As<CheckCustomerResponse>()
                    .ToListAsync();

            return result;
        }

        public async Task<long> CountAsync(string textSearch, IEnumerable<string> creatorIds)
        {
            var filter = GetFilter(textSearch, creatorIds);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<CheckCustomer> GetFilter(string textSearch, IEnumerable<string> creatorIds)
        {
            var filter = Builders<CheckCustomer>.Filter.Eq(x => x.IsDeleted, false);
            if (creatorIds.Any())
            {
                filter &= Builders<CheckCustomer>.Filter.In(x => x.Creator, creatorIds);
            }

            if (!string.IsNullOrEmpty(textSearch))
            {
                var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<CheckCustomer>.Filter.Regex(x => x.SaleInfomation.UserName, regex) |
                    Builders<CheckCustomer>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<CheckCustomer>.Filter.Regex(x => x.FileName, regex);
            }
            return filter;
        }
    }
}
