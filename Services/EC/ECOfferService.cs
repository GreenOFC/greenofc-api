using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Models.EC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.EC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public class ECOfferService : IScopedLifetime
    {
        private readonly ILogger<ECOfferService> _logger;
        private readonly IMapper _mapper;
        private readonly IMongoRepository<ECOfferData> _ecOfferCollection;
        private readonly IECOfferDataRepository _ecOfferDataRepository;

        public ECOfferService(
            ILogger<ECOfferService> logger,
            IMapper mapper,
            IMongoRepository<ECOfferData> ecOfferCollection,
            IECOfferDataRepository ecOfferDataRepository
            )
        {
            _logger = logger;
            _mapper = mapper;
            _ecOfferCollection = ecOfferCollection;
            _ecOfferDataRepository = ecOfferDataRepository;
        }

        public async Task<ECOfferResponse> CreateOffer(ECOfferDataDto request)
        {
            try
            {
                var ecOffer = _mapper.Map<ECOfferData>(request);
                await _ecOfferCollection.InsertOneAsync(ecOffer);

                var ecOfferResponse = new ECOfferResponse
                {
                    StatusCode = 200
                };

                return ecOfferResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ECOfferDataDto> GetAsync(string requestId)
        {
            try
            {
                var ecOffer = await _ecOfferCollection.FindOneAsync(x => x.RequestId == requestId && x.Code == ECReturnUpdateStatus.VALIDATED);
                if (ecOffer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(ECOfferData)));
                }

                var ecOfferDto = _mapper.Map<ECOfferDataDto>(ecOffer);
                return ecOfferDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<ECOfferData>> GetEcOfferList(PagingRequest pagingRequest)
        {
            try
            {
                var result = await _ecOfferDataRepository.GetList(pagingRequest);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
