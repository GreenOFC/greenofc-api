using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace _24hplusdotnetcore.Services.CRM
{
    public class DataCRMProcessingServices : IScopedLifetime
    {
        private readonly ILogger<DataCRMProcessingServices> _logger;
        private readonly IMongoCollection<DataCRMProcessing> _dataCRMProcessing;
        public DataCRMProcessingServices(ILogger<DataCRMProcessingServices> logger, IMongoDbConnection connection)
        {
            _logger = logger;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _dataCRMProcessing = database.GetCollection<DataCRMProcessing>(Common.MongoCollection.DataCRMProcessing);
        }

        public DataCRMProcessing InsertOne(DataCRMProcessing dataCRM)
        {
            var newData = new DataCRMProcessing();
            try
            {
                Expression<Func<DataCRMProcessing, bool>> filter = x => x.LeadSource == dataCRM.LeadSource &&
                    x.Status == dataCRM.Status &&
                    x.CustomerId == dataCRM.CustomerId &&
                    x.LeadCrmId == dataCRM.LeadCrmId;

                var entity = _dataCRMProcessing.Find(filter).FirstOrDefault();
                if(entity == null)
                {
                    _dataCRMProcessing.InsertOne(dataCRM);
                    return entity;
                }

                return dataCRM;
            }
            catch (Exception ex)
            {
                
                _logger.LogInformation("DataCRMProcessing InsertOne 45");
                _logger.LogError(ex, ex.Message);
            }
            return newData;
        }

        public long UpdateByCustomerId(DataCRMProcessing dataCRMProcessing, string Status)
        {
            try
            {
                dataCRMProcessing.Status = Status;
                var modifiedCount = _dataCRMProcessing.ReplaceOne(d => d.Id == dataCRMProcessing.Id, dataCRMProcessing).ModifiedCount;
                return modifiedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return -1;
            }
        }

        public List<DataCRMProcessing> GetDataCRMProcessings(string status)
        {
            var listDataCRMProcessing = new List<DataCRMProcessing>();
            try
            {
                listDataCRMProcessing = _dataCRMProcessing.Find(d => d.Status == status).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return listDataCRMProcessing;
        }

        public IEnumerable<DataCRMProcessing> InsertMany(IEnumerable<DataCRMProcessing> dataProcessings)
        {
            _dataCRMProcessing.InsertMany(dataProcessings);
            return dataProcessings;
        }
    }
}
