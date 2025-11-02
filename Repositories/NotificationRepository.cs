using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos.Notification;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface INotificationRepository : IMongoRepository<Notification>
    {
        Task<IEnumerable<GetNotificationResponse>> GetAsync(string userId, bool isUnread, string greenType, int pageIndex, int pageSize);
        Task<long> CountAsync(string userId, bool isUnread, string greenType);
    }
    public class NotificationRepository : MongoRepository<Notification>, INotificationRepository, IScopedLifetime
    {
        public NotificationRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetNotificationResponse>> GetAsync(string userId, bool isUnread, string greenType, int pageIndex, int pageSize)
        {
            var filter = GetFilter(userId, isUnread, greenType);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "UserName", 1 },
                    { "Type", 1 },
                    { "GreenType", 1},
                    { "RecordId", 1},
                    { "Title", 1},
                    { "Message", 1},
                    { "IsRead", 1},
                    { "CreatedDate", 1},
                    { "ModifiedDate", 1},
                };
            return await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.CreatedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .Project(projectMapping)
                    .As<GetNotificationResponse>()
                    .ToListAsync();
        }

        public async Task<long> CountAsync(string userId, bool isUnread, string greenType)
        {
            var filter = GetFilter(userId, isUnread, greenType);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }


        private FilterDefinition<Notification> GetFilter(string userId, bool isUnread, string greenType)
        {
            var filter = Builders<Notification>.Filter.Eq(x => x.UserId, userId);
            if (isUnread)
            {
                filter &= Builders<Notification>.Filter.Eq(x => x.IsRead, false);
            }
            if (!string.IsNullOrEmpty(greenType))
            {
                filter &= Builders<Notification>.Filter.Eq(x => x.GreenType, greenType);
            }
            return filter;
        }
    }
}
