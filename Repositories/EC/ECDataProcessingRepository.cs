using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.EC;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.EC
{
    public interface IECDataProcessingRepository : IMongoRepository<ECDataProcessing>
    {
        Task<ECDataProcessing> GetECDataProcessingByCustomerId(string customerId);
        Task UpdatePayload(string customerId, string request);
        Task UpdateResponse(string id, string repsonse, string status);
        Task Insert(ECDataProcessing eCDataProcessing);
        Task<IEnumerable<ECDataProcessing>> GetListByCustomerId(string customerId);
    }

    public class ECDataProcessingRepository : MongoRepository<ECDataProcessing>, IECDataProcessingRepository, IScopedLifetime
    {
        private readonly ILogger<ECDataProcessingRepository> _logger;

        public ECDataProcessingRepository(ILogger<ECDataProcessingRepository> logger, IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
            _logger = logger;
        }

        public async Task<ECDataProcessing> GetECDataProcessingByCustomerId(string customerId)
        {
            try
            {
                var query = _collection.Aggregate()
                    .Match(x => x.CustomerId == customerId && x.Status == EcDataProcessingStatus.DRAFT)
                    .SortByDescending(x => x.CreatedDate)
                   .FirstOrDefaultAsync();

                return await query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Insert(ECDataProcessing eCDataProcessing)
        {
            try
            {
                await InsertOneAsync(eCDataProcessing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdatePayload(string customerId, string payload)
        {
            try
            {
                UpdateOptions updateOptions = new UpdateOptions
                {
                    IsUpsert = true
                };

                var update = Builders<ECDataProcessing>.Update
                    .Set(x => x.PayLoad, payload)
                    .Set(x => x.CustomerId, customerId);

                await UpdateOneAsync(x => x.CustomerId == customerId, update, updateOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task UpdateResponse(string id, string response, string status)
        {
            try
            {
                var ecdataProcessingDetail = await base.FindOneAsync(x => x.Id == id);

                if (null != ecdataProcessingDetail)
                {
                    var update = Builders<ECDataProcessing>.Update
                    .Set(x => x.Response, response)
                    .Set(x => x.Status, status);

                    await UpdateOneAsync(x => x.Id == id, update);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ECDataProcessing>> GetListByCustomerId(string customerId)
        {
            try
            {
                var ecdataProcessingQuery = await base.FilterByAsync(x => x.CustomerId == customerId);
                return ecdataProcessingQuery.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
