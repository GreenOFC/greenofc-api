using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models.EC;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.EC
{

    public interface IECOfferDataRepository
    {
        Task<ECOfferData> GetDetailByRequestId(string requestId);
        Task ReplaceByRequestId(ECOfferData request);
        Task Create(ECOfferData request);
        Task<PagingResponse<ECOfferData>> GetList(PagingRequest pagingRequest);

    }

    public class ECOfferDataRepository : IECOfferDataRepository, IScopedLifetime
    {
        private readonly ILogger<ECOfferDataRepository> _logger;
        private readonly IMongoRepository<ECOfferData> _ecofferDataRepository;


        public ECOfferDataRepository(ILogger<ECOfferDataRepository> logger, IMongoRepository<ECOfferData> ecofferDataRepository)
        {
            _logger = logger;
            _ecofferDataRepository = ecofferDataRepository;
        }

        public async Task Create(ECOfferData request)
        {
            try
            {
                await _ecofferDataRepository.InsertOneAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECOfferData> GetDetailByRequestId(string requestId)
        {
            try
            {
                return await _ecofferDataRepository.FindOneAsync(x => x.RequestId == requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ReplaceByRequestId(ECOfferData request)
        {
            try
            {
                await _ecofferDataRepository.ReplaceOneAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<ECOfferData>> GetList(PagingRequest pagingRequest)
        {
            try
            {
                var filter = GetFilter(pagingRequest.TextSearch, pagingRequest.GetFromDate(), pagingRequest.GetToDate());
                var queryData = await _ecofferDataRepository
                                .GetCollection()
                                .Aggregate()
                                .Match(filter)
                                .SortByDescending(x => x.CreatedDate)
                                .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                                .Limit(pagingRequest.PageSize)
                                .ToListAsync();

                var countData = await _ecofferDataRepository
                                .GetCollection()
                                .Aggregate()
                                .Match(filter)
                                .ToListAsync();

                var result = new PagingResponse<ECOfferData>
                {
                    TotalRecord = countData.Count(),
                    Data = queryData,
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private FilterDefinition<ECOfferData> GetFilter(string textSearch, DateTime? fromDate, DateTime? toDate)
        {
            var filter = Builders<ECOfferData>.Filter.Empty;
            if (!string.IsNullOrEmpty(textSearch))
            {
                filter &= Builders<ECOfferData>.Filter.Regex(x => x.RequestId, new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i"));
            }
            if (fromDate.HasValue)
            {
                filter &= Builders<ECOfferData>.Filter.Gte(x => x.ModifiedDate, fromDate);
            }
            if (toDate.HasValue)
            {
                filter &= Builders<ECOfferData>.Filter.Lte(x => x.ModifiedDate, toDate);
            }
            return filter;
        }
    }
}
