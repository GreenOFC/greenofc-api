using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.Lead;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.Lead
{

    public interface IHyperLeadRepository : IMongoRepository<HyperLead>
    {
        Task<HyperLead> Create(HyperLead leadsource);
        Task<IEnumerable<HyperLead>> GetList(IEnumerable<string> creators, PagingRequest pagingRequest);
        Task<long> CountLead(IEnumerable<string> creators, PagingRequest pagingRequest);
    }

    public class HyperLeadRepository : MongoRepository<HyperLead>, IHyperLeadRepository, IScopedLifetime
    {
        public HyperLeadRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }
        public async Task<HyperLead> Create(HyperLead leadsource)
        {
            try
            {
                await _collection.InsertOneAsync(leadsource);
                return leadsource;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HyperLead>> GetList(IEnumerable<string> creators, PagingRequest pagingRequest)
        {
            try
            {
                var filter = GetFilter(creators, pagingRequest);

                var leadvpsList = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.CreatedDate)
                    .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                    .Limit(pagingRequest.PageSize)
                    .ToListAsync();

                return leadvpsList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> CountLead(IEnumerable<string> creators, PagingRequest pagingRequest)
        {
            try
            {
                var filter = GetFilter(creators, pagingRequest);

                var totalLead = await _collection
                    .Aggregate()
                    .Match(filter)
                    .ToListAsync();

                return totalLead.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private FilterDefinition<HyperLead> GetFilter(IEnumerable<string> userIds, PagingRequest pagingRequest)
        {
            var filter = Builders<HyperLead>.Filter.Ne(x => x.IsDeleted, true);

            if (userIds?.Any() == true)
            {
                filter &= Builders<HyperLead>.Filter.In(x => x.Creator, userIds);
            }


            if (!string.IsNullOrEmpty(pagingRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<HyperLead>.Filter.Regex(x => x.TransactionId, regex) |
                    Builders<HyperLead>.Filter.Regex(x => x.OfferId, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.Sale))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<HyperLead>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<HyperLead>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<HyperLead>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<HyperLead>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.Asm))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.Asm.ConvertSpecialCharacters()}/i");
                filter &= Builders<HyperLead>.Filter.Regex(x => x.AsmInfo.FullName, regex) |
                    Builders<HyperLead>.Filter.Regex(x => x.AsmInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.PosManager))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<HyperLead>.Filter.Regex(x => x.PosInfo.Name, regex);
            }
            return filter;
        }
    }
}
