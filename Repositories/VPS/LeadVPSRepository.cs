using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.VPS;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.VPS
{

    public interface ILeadVpsRepository
    {
        Task<LeadVps> GetDetailAsync(string id);
        Task<LeadVps> Create(LeadVps leadsource);
        Task Update(LeadVps leadsource);
        Task Delete(string id);
        Task<IEnumerable<LeadVps>> GetList(IEnumerable<string> creators, PagingRequest pagingRequest);
        Task<long> CountLead(IEnumerable<string> creators, PagingRequest pagingRequest);
    }

    public class LeadVpsRepository : ILeadVpsRepository, IScopedLifetime
    {
        private readonly ILogger<LeadVpsRepository> _logger;
        private readonly IMongoRepository<LeadSource> _leadsourceRepository;
        private readonly IMongoRepository<LeadVps> _leadVpsRepository;

        public LeadVpsRepository(
            ILogger<LeadVpsRepository> logger,
            IMongoRepository<LeadSource> leadsourceRepository,
            IMongoRepository<LeadVps> leadVpsRepository
            )
        {
            _logger = logger;
            _leadsourceRepository = leadsourceRepository;
            _leadVpsRepository = leadVpsRepository;
        }

        public async Task<LeadVps> Create(LeadVps leadsource)
        {
            try
            {
                await _leadsourceRepository.InsertOneAsync(leadsource);
                return leadsource;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _leadsourceRepository.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadVps> GetDetailAsync(string id)
        {
            try
            {
                var leadsourceDetail = await _leadsourceRepository.GetCollection().OfType<LeadVps>().FindAsync(x => x.Id == id);
                return leadsourceDetail.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LeadVps>> GetList(IEnumerable<string> creators, PagingRequest pagingRequest)
        {
            try
            {
                var filter = GetFilter(creators, pagingRequest);

                var leadvpsList = await _leadsourceRepository.GetCollection().OfType<LeadVps>()
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
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<long> CountLead(IEnumerable<string> creators, PagingRequest pagingRequest)
        {
            try
            {
                var filter = GetFilter(creators, pagingRequest);

                var totalLead = await _leadsourceRepository.GetCollection().OfType<LeadVps>()
                    .Aggregate()
                    .Match(filter)
                    .ToListAsync();

                return totalLead.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Update(LeadVps leadsource)
        {
            try
            {
                var update = Builders<LeadVps>.Update
                                 .Set(x => x.IdCard, leadsource.IdCard)
                                 .Set(x => x.FullName, leadsource.FullName)
                                 .Set(x => x.ModifiedDate, DateTime.Now)
                                 .Set(x => x.Modifier, leadsource.Modifier);

                await _leadVpsRepository.UpdateOneAsync(x => x.Id == leadsource.Id, update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private FilterDefinition<LeadVps> GetFilter(IEnumerable<string> userIds, PagingRequest pagingRequest)
        {
            var filter = Builders<LeadVps>.Filter.Ne(x => x.IsDeleted, true);

            if (!string.IsNullOrEmpty(pagingRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVps>.Filter.Regex(x => x.FullName, regex) |
                    Builders<LeadVps>.Filter.Regex(x => x.IdCard, regex);
            }
            if (userIds?.Any() == true)
            {
                filter &= Builders<LeadVps>.Filter.In(x => x.Creator, userIds);
            }


            if (!string.IsNullOrEmpty(pagingRequest.Sale))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVps>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<LeadVps>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVps>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<LeadVps>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(pagingRequest.PosManager))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVps>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                    Builders<LeadVps>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
            }
            return filter;
        }
    }
}
