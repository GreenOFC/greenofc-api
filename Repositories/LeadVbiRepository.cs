using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadVbis;
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
    public interface ILeadVbiRepository: IMongoRepository<LeadSource>
    {
        Task<IEnumerable<GetLeadVbiResponse>> GetAsync(IEnumerable<string> userIds, PagingRequest pagingRequest);
        Task<long> CountAsync(IEnumerable<string> userIds, PagingRequest pagingRequest);
        Task<GetDetailLeadVbiResponse> GetDetailAsync(string id);
    }
    public class LeadVbiRepository: MongoRepository<LeadSource>, ILeadVbiRepository, IScopedLifetime
    {
        public LeadVbiRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<GetLeadVbiResponse>> GetAsync(IEnumerable<string> userIds, PagingRequest pagingRequest)
        {
            var filter = GetFilter(userIds, pagingRequest);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Phone", 1 },
                    { "ExtraPhone", 1 },
                    { "TemporaryAddress", 1 },
                    { "Status", 1 },
                    { "CreatedDate", 1 },
                    { "Creator", 1 },
                    { "ModifiedDate", 1 },
                    { "PosInfo", 1},
                    { "TeamLeadInfo", 1},
                    { "SaleChanelInfo", 1},
                    { "SaleInfo", "$SaleInfomation"},
                };
            return await _collection
                .OfType<LeadVbi>()
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                .Limit(pagingRequest.PageSize)
                .Project(projectMapping)
                .As<GetLeadVbiResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(IEnumerable<string> userIds, PagingRequest pagingRequest)
        {
            var filter = GetFilter(userIds, pagingRequest);

            var total = await _collection
                .OfType<LeadVbi>()
                .Find(filter)
                .CountDocumentsAsync();

            return total;
        }

        public async Task<GetDetailLeadVbiResponse> GetDetailAsync(string id)
        {
            var filter = Builders<LeadVbi>.Filter.Ne(x => x.IsDeleted, true);
            filter &= Builders<LeadVbi>.Filter.Eq(x => x.Id, id);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Phone", 1 },
                    { "ExtraPhone", 1 },
                    { "TemporaryAddress", 1 },
                    { "Status", 1 },
                    { "CreatedDate", 1 },
                    { "Creator", 1 },
                    { "ModifiedDate", 1 },
                    { "PosInfo", 1},
                    { "TeamLeadInfo", 1},
                    { "SaleChanelInfo", 1},
                    { "SaleInfo", "$SaleInfomation"},
                };
            return await _collection
                .OfType<LeadVbi>()
                .Aggregate()
                .Match(filter)
                .Project(projectMapping)
                .As<GetDetailLeadVbiResponse>()
                .FirstOrDefaultAsync();
        }

        private FilterDefinition<LeadVbi> GetFilter(IEnumerable<string> userIds, PagingRequest pagingRequest)
        {
            var filter = Builders<LeadVbi>.Filter.Ne(x => x.IsDeleted, true);
            filter &= Builders<LeadVbi>.Filter.Gte(x => x.ModifiedDate, pagingRequest.GetFromDate());
            filter &= Builders<LeadVbi>.Filter.Lte(x => x.ModifiedDate, pagingRequest.GetToDate());

            if (!string.IsNullOrEmpty(pagingRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVbi>.Filter.Regex(x => x.FullName, regex) |
                    Builders<LeadVbi>.Filter.Regex(x => x.IdCard, regex);
            }
            if (userIds?.Any() == true)
            {
                filter &= Builders<LeadVbi>.Filter.In(x => x.Creator, userIds);
            }
            if (!string.IsNullOrEmpty(pagingRequest.Sale))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVbi>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<LeadVbi>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVbi>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<LeadVbi>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.PosManager))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVbi>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                    Builders<LeadVbi>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
            }
            return filter;
        }
    }
}
