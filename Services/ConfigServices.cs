using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public class ConfigServices : IScopedLifetime
    {
        private readonly ILogger<NotificationServices> _logger;
        private readonly IMongoCollection<ConfigModel> _collection;
        public ConfigServices(IMongoDbConnection connection, ILogger<NotificationServices> logger)
        {
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<ConfigModel>(MongoCollection.Config);
            _logger = logger;
        }
        public ConfigModel FindOneByKey(string key)
        {
            try
            {
                return _collection.Find(x => x.Key == key).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task UpsertAsync<T>(string key, T value)
        {
            var config = await _collection.Find(x => x.Key == key).FirstOrDefaultAsync();
            if(config == null)
            {
                await _collection.InsertOneAsync(new ConfigModel { Key = key, Value = value });
                return;
            }

            var filter = Builders<ConfigModel>.Filter.Eq(x => x.Id, config.Id);
            var update = Builders<ConfigModel>.Update.Set(x => x.Value, value);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
