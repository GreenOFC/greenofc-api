using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Models.EC;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public class ECNotificationService : IScopedLifetime
    {
        private readonly ILogger<ECNotificationService> _logger;
        private readonly IMongoRepository<ECNotification> _ecNotificationCollection;
        private readonly IMapper _mapper;

        public ECNotificationService(
             ILogger<ECNotificationService> logger,
            IMongoRepository<ECNotification> ecNotificationCollection,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _ecNotificationCollection = ecNotificationCollection;
        }

        public async Task<ECUpdateStatusResponse> Create(ECNotification request)
        {
            try
            {
                var notification = _mapper.Map<ECNotification>(request);
                await _ecNotificationCollection.InsertOneAsync(notification);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
