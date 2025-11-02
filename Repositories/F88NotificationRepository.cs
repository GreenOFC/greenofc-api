using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IF88NotificationRepository : IMongoRepository<F88Notification>
    {
        Task<long> CountAsync(string textSearch, string status);
        Task<IEnumerable<F88Notification>> GetNotiAsync(string textSearch, string status, int pageIndex, int pageSize);
    }
    public class F88NotificationRepository : MongoRepository<F88Notification>, IF88NotificationRepository, IScopedLifetime
    {
        public F88NotificationRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {

        }

        public async Task<IEnumerable<F88Notification>> GetNotiAsync(string textSearch, string status, int pageIndex, int pageSize)
        {
            var filter = GetNotiFilter(textSearch, status);
            var leadF88s = await _collection.Find(filter)
                .SortByDescending(c => c.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return leadF88s;
        }

        public async Task<long> CountAsync(string textSearch, string status)
        {
            var filter = GetNotiFilter(textSearch, status);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<F88Notification> GetNotiFilter(string textSearch, string status)
        {
            var filter = Builders<F88Notification>.Filter.Empty;
            if (!string.IsNullOrEmpty(textSearch))
            {
                filter &= Builders<F88Notification>.Filter.Regex(x => x.TransactionId, new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i"));
            }
            if (!string.IsNullOrEmpty(status))
            {
                filter &= Builders<F88Notification>.Filter.Eq(x => x.Status, status);
            }

            return filter;
        }
    }
}
