using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.LeadHomes;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ILeadHomeRepository: IMongoRepository<LeadSource>
    {
        Task<IEnumerable<GetLeadHomeResponse>> GetAsync(IEnumerable<string> userIds, GetLeadHomeRequest getLeadHomeRequest);
        Task<long> CountAsync(IEnumerable<string> userIds, GetLeadHomeRequest getLeadHomeRequest);
        Task<GetDetailLeadHomeResponse> GetDetailAsync(string id);
    }

    public class LeadHomeRepository : MongoRepository<LeadSource>, ILeadHomeRepository, IScopedLifetime
    {
        public LeadHomeRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetLeadHomeResponse>> GetAsync(IEnumerable<string> userIds, GetLeadHomeRequest getLeadHomeRequest)
        {
            var filter = GetFilter(userIds, getLeadHomeRequest);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Phone", 1 },
                    { "TemporaryAddress", 1 },
                    { "CreatedDate", 1 },
                    { "Creator", 1 },
                    { "ModifiedDate", 1 },
                    { "PosInfo", 1},
                    { "TeamLeadInfo", 1},
                    { "SaleChanelInfo", 1},
                    { "SaleInfo", "$SaleInfomation"},
                };
            return await _collection
                .OfType<LeadHome>()
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((getLeadHomeRequest.PageIndex - 1) * getLeadHomeRequest.PageSize)
                .Limit(getLeadHomeRequest.PageSize)
                .Project(projectMapping)
                .As<GetLeadHomeResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(IEnumerable<string> userIds, GetLeadHomeRequest getLeadHomeRequest)
        {
            var filter = GetFilter(userIds, getLeadHomeRequest);

            var total = await _collection
                .OfType<LeadHome>()
                .Find(filter)
                .CountDocumentsAsync();

            return total;
        }

        public async Task<GetDetailLeadHomeResponse> GetDetailAsync(string id)
        {
            var filter = Builders<LeadHome>.Filter.Ne(x => x.IsDeleted, true);
            filter &= Builders<LeadHome>.Filter.Eq(x => x.Id, id);
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Phone", 1 },
                    { "TemporaryAddress", 1 },
                    { "CreatedDate", 1 },
                    { "Creator", 1 },
                    { "ModifiedDate", 1 },
                    { "SaleInfo", "$SaleInfomation"},
                };
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

            return await _collection
                .OfType<LeadHome>()
                .Aggregate()
                .Match(filter)
                .Project(projectMapping)
                .As<GetDetailLeadHomeResponse>()
                .FirstOrDefaultAsync();
        }

        private FilterDefinition<LeadHome> GetFilter(IEnumerable<string> userIds, GetLeadHomeRequest getLeadHomeRequest)
        {
            var filter = Builders<LeadHome>.Filter.Ne(x => x.IsDeleted, true);
            filter &= Builders<LeadHome>.Filter.Gte(x => x.ModifiedDate, getLeadHomeRequest.GetFromDate());
            filter &= Builders<LeadHome>.Filter.Lte(x => x.ModifiedDate, getLeadHomeRequest.GetToDate());

            if (!string.IsNullOrEmpty(getLeadHomeRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{getLeadHomeRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadHome>.Filter.Regex(x => x.FullName, regex) |
                    Builders<LeadHome>.Filter.Regex(x => x.IdCard, regex);
            }
            if (userIds?.Any() == true)
            {
                filter &= Builders<LeadHome>.Filter.In(x => x.Creator, userIds);
            }
            
            
            if (!string.IsNullOrEmpty(getLeadHomeRequest.Sale))
            {
                var regex = new BsonRegularExpression($"/{getLeadHomeRequest.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadHome>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<LeadHome>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadHomeRequest.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{getLeadHomeRequest.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadHome>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<LeadHome>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadHomeRequest.PosManager))
            {
                var regex = new BsonRegularExpression($"/{getLeadHomeRequest.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadHome>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                    Builders<LeadHome>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
            }
            return filter;
        }
    }
}
