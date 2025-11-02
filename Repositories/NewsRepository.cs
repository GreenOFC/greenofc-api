using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.News;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface INewsRepository : IMongoRepository<News>
    {
        Task<IEnumerable<GetNewsResponse>> GetAsync(string userLoginId, string type, string textSearch, int pageIndex, int pageSize);
        Task<long> CountAsync(string userLoginId, string type, string textSearch);
        Task<GetDetailNewsResponse> GetAsync(string id);
    }
    public class NewsRepository : MongoRepository<News>, INewsRepository, IScopedLifetime
    {
        private readonly IMongoRepository<GroupNotificationUser> _groupNotificationUserRepository;

        public NewsRepository(IMongoDbConnection mongoDbConnection, IMongoRepository<GroupNotificationUser> groupNotificationUserRepository) : base(mongoDbConnection)
        {
            _groupNotificationUserRepository = groupNotificationUserRepository;
        }

        public async Task<IEnumerable<GetNewsResponse>> GetAsync(string userLoginId, string type, string textSearch, int pageIndex, int pageSize)
        {
            var filter = GetFilter(type, textSearch);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "Title", 1 },
                    { "AvatarUrl", 1 },
                    { "Type", 1},
                    { "Tag", 1},
                    { "Desc", 1},
                    { "Creator", "$CreatorUser.FullName"},
                    { "CreatedDate", 1},
                    { "GroupNotificationIds", 1}
                };
            IEnumerable<GetNewsResponse> listOfNews = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Lookup("Users", "Creator", "_id", "n")
                    .Unwind("CreatorUser", unwindOption)
                    .Project(projectMapping)
                    .As<GetNewsResponse>()
                    .ToListAsync();

            // get list of groupnotification to which user is belong
            var subQuery = (await _groupNotificationUserRepository.FilterByAsync(x => x.UserId == userLoginId)).ToList();
            var groupNotificationids = subQuery.Select(x => x.GroupNotificationId);

            var lastResult = listOfNews
                .Where( x => x.GroupNotificationIds != null &&  x.GroupNotificationIds.Any( y => groupNotificationids.Contains(y)));

            lastResult = lastResult.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            return lastResult;
        }

        public async Task<long> CountAsync(string userLoginId, string type, string textSearch)
        {
            var filter = GetFilter(type, textSearch);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "Title", 1 },
                    { "AvatarUrl", 1 },
                    { "Type", 1},
                    { "Tag", 1},
                    { "Creator", "$CreatorUser.FullName"},
                    { "CreatedDate", 1},
                    { "GroupNotificationIds", 1}
                };

            IEnumerable<GetNewsResponse> listOfNews = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Lookup("Users", "Creator", "_id", "n")
                    .Unwind("CreatorUser", unwindOption)
                    .Project(projectMapping)
                    .As<GetNewsResponse>()
                    .ToListAsync();

            // get list of groupnotification to which user is belong
            var subQuery = (await _groupNotificationUserRepository.FilterByAsync(x => x.UserId == userLoginId)).ToList();
            var groupNotificationids = subQuery.Select(x => x.GroupNotificationId);

            var lastResult = listOfNews
                .Where(x => x.GroupNotificationIds != null && x.GroupNotificationIds.Any(y => groupNotificationids.Contains(y)));

            return lastResult.Count();
        }

        private FilterDefinition<News> GetFilter(string type, string textSearch)
        {
            var filter = Builders<News>.Filter.Eq(banner => banner.IsDeleted, false);
            if (!string.IsNullOrEmpty(textSearch))
            {
                var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<News>.Filter.Regex(x => x.Title, regex);
            }
            if (!string.IsNullOrEmpty(type))
            {
                filter &= Builders<News>.Filter.Eq(x => x.Type, type);
            }
            return filter;
        }

        public async Task<GetDetailNewsResponse> GetAsync(string id)
        {
            var filter = Builders<News>.Filter.Eq(x => x.IsDeleted, false);
            filter &= Builders<News>.Filter.Eq(x => x.Id, id);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "Title", 1 },
                    { "AvatarUrl", 1 },
                    { "Content", 1},
                    { "Desc", 1},
                    { "Type", 1},
                    { "Tag", 1},
                    { "Creator", "$CreatorUser.FullName"},
                    { "CreatedDate", 1},
                    { "GroupNotificationIds", 1},
                };
            var news = await _collection
                    .Aggregate()
                    .Match(filter)
                    .Lookup("Users", "Creator", "_id", "CreatorUser")
                    .Unwind("CreatorUser", unwindOption)
                    .Project(projectMapping)
                    .As<GetDetailNewsResponse>()
                    .FirstOrDefaultAsync();

            return news;
        }
    }
}
