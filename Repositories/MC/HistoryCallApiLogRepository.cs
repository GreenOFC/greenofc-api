using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.MC
{
    public interface IHistoryCallApiLogRepository : IMongoRepository<HistoryCallApiLog>
    {
        Task<IEnumerable<HistoryCallApiLogResponse>> GetAsync(HistoryCallApiLogRequest request, IEnumerable<string> userIds);

        Task<long> CountAsync(HistoryCallApiLogRequest request, IEnumerable<string> userIds);
    }

    public class HistoryCallApiLogRepository : MongoRepository<HistoryCallApiLog>, IHistoryCallApiLogRepository, IScopedLifetime
    {
        public HistoryCallApiLogRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<HistoryCallApiLogResponse>> GetAsync(HistoryCallApiLogRequest request, IEnumerable<string> userIds)
        {
            var filter = GetFilter(request, userIds);

            var result = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Limit(request.PageSize)
                    .As<HistoryCallApiLogResponse>()
                    .ToListAsync();

            return result;
        }

        public async Task<long> CountAsync(HistoryCallApiLogRequest request, IEnumerable<string> userIds)
        {
            var filter = GetFilter(request, userIds);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<HistoryCallApiLog> GetFilter(HistoryCallApiLogRequest request, IEnumerable<string> userIds)
        {
            var filter = Builders<HistoryCallApiLog>.Filter.Eq(x => x.IsDeleted, false);
            filter &= Builders<HistoryCallApiLog>.Filter.Gte(x => x.ModifiedDate, request.GetFromDate());
            filter &= Builders<HistoryCallApiLog>.Filter.Lte(x => x.ModifiedDate, request.GetToDate());


            if (userIds?.Any() == true)
            {
                filter &= Builders<HistoryCallApiLog>.Filter.In(x => x.Creator, userIds);
            }

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                var regexText = new BsonRegularExpression($"/{request.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<HistoryCallApiLog>.Filter.Regex(x => x.Payload, regexText);
            }
            if (!string.IsNullOrEmpty(request.Action))
            {
                filter &= Builders<HistoryCallApiLog>.Filter.Eq(x => x.Action, request.Action);
            }
            if (!string.IsNullOrEmpty(request.GreenType))
            {
                filter &= Builders<HistoryCallApiLog>.Filter.Eq(x => x.GreenType, request.GreenType);
            }

            if (!string.IsNullOrEmpty(request.Sale))
            {
                var regex = new BsonRegularExpression($"/{request.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<HistoryCallApiLog>.Filter.Regex(x => x.SaleInfo.FullName, regex)
                        | Builders<HistoryCallApiLog>.Filter.Regex(x => x.SaleInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(request.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{request.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<HistoryCallApiLog>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex)
                        | Builders<HistoryCallApiLog>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(request.Asm))
            {
                var regex = new BsonRegularExpression($"/{request.Asm.ConvertSpecialCharacters()}/i");
                filter &= Builders<HistoryCallApiLog>.Filter.Regex(x => x.AsmInfo.FullName, regex)
                        | Builders<HistoryCallApiLog>.Filter.Regex(x => x.AsmInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(request.PosManager))
            {
                var regex = new BsonRegularExpression($"/{request.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<HistoryCallApiLog>.Filter.Regex(x => x.PosInfo.Name, regex);
            }

            return filter;
        }

    }
}
