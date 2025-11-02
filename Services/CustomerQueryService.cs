using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Services.MC;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public class CustomerQueryService: IScopedLifetime
    {
        private readonly ILogger<CustomerQueryService> _logger;
        private readonly IMongoCollection<Customer> _collection;
        public CustomerQueryService(IMongoDbConnection connection,
        ILogger<CustomerQueryService> logger)
        {
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<Customer>(MongoCollection.CustomerCollection);
            _logger = logger;
        }
        public async Task<IEnumerable<Customer>> GetByCrmIdsAsync(IEnumerable<string> crmIds)
        {
            return await _collection.Find(c => !c.IsDeleted && crmIds.Contains(c.CRMId)).ToListAsync();
        }

        public async Task<Customer> GetCustomerAsync(string Id)
        {
            try
            {
                return await _collection.Find(c => !c.IsDeleted && c.Id == Id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<Customer>> GetMAFCProcessingAsync()
        {
            try
            {
                return await _collection.Find(c => !c.IsDeleted
                    && c.GreenType == GreenType.GreenA
                    && c.Status == CustomerStatus.PROCESSING
                    && (c.Result.ReturnStatus == "QDE" || c.Result.ReturnStatus == "BDE")
                    ).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
