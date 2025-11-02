using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.LeadOkVays;
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
    public interface ILeadOkVayRepository: IMongoRepository<LeadSource>
    {
        Task<IEnumerable<GetLeadOkVayResponse>> GetAsync(IEnumerable<string> userIds, GetLeadOkVayRequest getLeadOkVayRequest);
        Task<long> CountAsync(IEnumerable<string> userIds, GetLeadOkVayRequest getLeadOkVayRequest);
        Task<GetDetailLeadOkVayResponse> GetDetailAsync(string id);
    }
    public class LeadOkVayRepository : MongoRepository<LeadSource>, ILeadOkVayRepository, IScopedLifetime
    {
        public LeadOkVayRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetLeadOkVayResponse>> GetAsync(IEnumerable<string> userIds, GetLeadOkVayRequest getLeadOkVayRequest)
        {
            var filter = GetFilter(userIds, getLeadOkVayRequest);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Phone", 1 },
                    { "ExtraPhone", 1 },
                    { "TemporaryAddress", 1 },
                    { "Debt", 1 },
                    { "DebtId", 1 },
                    { "Income", 1 },
                    { "IncomeId", 1 },
                    { "CreatedDate", 1 },
                    { "Creator", 1 },
                    { "ModifiedDate", 1 },
                    { "PosInfo", 1},
                    { "TeamLeadInfo", 1},
                    { "Result", 1},
                    { "SaleInfo", "$SaleInfomation"},
                };
            return await _collection
                .OfType<LeadOkVay>()
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((getLeadOkVayRequest.PageIndex - 1) * getLeadOkVayRequest.PageSize)
                .Limit(getLeadOkVayRequest.PageSize)
                .Project(projectMapping)
                .As<GetLeadOkVayResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(IEnumerable<string> userIds, GetLeadOkVayRequest getLeadOkVayRequest)
        {
            var filter = GetFilter(userIds, getLeadOkVayRequest);

            var total = await _collection
                .OfType<LeadOkVay>()
                .Find(filter)
                .CountDocumentsAsync();

            return total;
        }

        public async Task<GetDetailLeadOkVayResponse> GetDetailAsync(string id)
        {
            var filter = Builders<LeadOkVay>.Filter.Ne(x => x.IsDeleted, true);
            filter &= Builders<LeadOkVay>.Filter.Eq(x => x.Id, id);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Phone", 1 },
                    { "ExtraPhone", 1 },
                    { "TemporaryAddress", 1 },
                    { "Debt", 1 },
                    { "DebtId", 1 },
                    { "Income", 1 },
                    { "IncomeId", 1 },
                    { "CreatedDate", 1 },
                    { "Creator", 1 },
                    { "ModifiedDate", 1 },
                    { "PosInfo", 1},
                    { "TeamLeadInfo", 1},
                    { "SaleInfo", "$SaleInfomation"},
                    { "Result", 1},
                };
            return await _collection
                .OfType<LeadOkVay>()
                .Aggregate()
                .Match(filter)
                .Project(projectMapping)
                .As<GetDetailLeadOkVayResponse>()
                .FirstOrDefaultAsync();
        }

        private FilterDefinition<LeadOkVay> GetFilter(IEnumerable<string> userIds, GetLeadOkVayRequest getLeadOkVayRequest)
        {
            var filter = Builders<LeadOkVay>.Filter.Ne(x => x.IsDeleted, true);
            filter &= Builders<LeadOkVay>.Filter.Gte(x => x.ModifiedDate, getLeadOkVayRequest.GetFromDate());
            filter &= Builders<LeadOkVay>.Filter.Lte(x => x.ModifiedDate, getLeadOkVayRequest.GetToDate());
            if (!string.IsNullOrEmpty(getLeadOkVayRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{getLeadOkVayRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadOkVay>.Filter.Regex(x => x.FullName, regex) |
                    Builders<LeadOkVay>.Filter.Regex(x => x.IdCard, regex);
            }
            if (userIds?.Any() == true)
            {
                filter &= Builders<LeadOkVay>.Filter.In(x => x.Creator, userIds);
            }
            if (!string.IsNullOrEmpty(getLeadOkVayRequest.Sale))
            {
                var regex = new BsonRegularExpression($"/{getLeadOkVayRequest.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadOkVay>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<LeadOkVay>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadOkVayRequest.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{getLeadOkVayRequest.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadOkVay>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<LeadOkVay>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadOkVayRequest.PosManager))
            {
                var regex = new BsonRegularExpression($"/{getLeadOkVayRequest.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadOkVay>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                    Builders<LeadOkVay>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
            }
            return filter;
        }
    }
}
