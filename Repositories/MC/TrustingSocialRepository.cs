using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelResponses.MC;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.MC
{
    public interface ITrustingSocialRepository
    {
        Task Create(MCTrustingSocial request);
        Task<IEnumerable<TrustingSocialResponse>> GetList(PagingRequest pagingRequest, IEnumerable<string> creators);
        Task<long> Count(PagingRequest pagingRequest, IEnumerable<string> creators);
    }

    public class TrustingSocialRepository : ITrustingSocialRepository, IScopedLifetime
    {
        private readonly ILogger<TrustingSocialRepository> _logger;
        private readonly IMongoRepository<MCTrustingSocial> _mcTrustingSocialRepository;

        public TrustingSocialRepository(ILogger<TrustingSocialRepository> logger, IMongoRepository<MCTrustingSocial> mcTrustingSocialRepository)
        {
            _logger = logger;
            _mcTrustingSocialRepository = mcTrustingSocialRepository;
        }

        public async Task Create(MCTrustingSocial request)
        {
            try
            {
                await _mcTrustingSocialRepository.InsertOneAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<TrustingSocialResponse>> GetList(PagingRequest pagingRequest, IEnumerable<string> creators)
        {
            try
            {
                var filter = GetFilter(pagingRequest, creators);

                var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };

                var projectMapping = new BsonDocument()
                {
                    // trusting social
                    { "PayLoad", 1 },
                    { "Response", 1 },
                    { "Creator", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    // sale
                    { "SaleInfo._id", 1},
                    { "SaleInfo.FullName", 1},
                    { "SaleInfo.UserName", 1}
                };

                var result = await _mcTrustingSocialRepository.GetCollection()
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.CreatedDate)
                    .Lookup("Users", "Creator", "_id", "SaleInfo")
                    .Unwind("SaleInfo", unwindOption)
                    .Project(projectMapping)
                    .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                    .Limit(pagingRequest.PageSize)
                    .As<TrustingSocialResponse>()
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<long> Count(PagingRequest pagingRequest, IEnumerable<string> creators)
        {
            try
            {
                var filter = GetFilter(pagingRequest, creators);

                var result = await _mcTrustingSocialRepository.GetCollection()
                    .Find(filter).CountDocumentsAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private FilterDefinition<MCTrustingSocial> GetFilter(PagingRequest pagingRequest, IEnumerable<string> creators)
        {
            var filter = Builders<MCTrustingSocial>.Filter.Eq(x => x.IsDeleted, false);
            filter &= Builders<MCTrustingSocial>.Filter.Gte(x => x.ModifiedDate, pagingRequest.GetFromDate());
            filter &= Builders<MCTrustingSocial>.Filter.Lte(x => x.ModifiedDate, pagingRequest.GetToDate());

            if (creators.Any())
            {
                filter &= Builders<MCTrustingSocial>.Filter.In(x => x.Creator, creators);
            }
            if (!string.IsNullOrEmpty(pagingRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{pagingRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<MCTrustingSocial>.Filter.Regex(x => x.PayLoad, regex);
            }

            return filter;
        }
    }
}
