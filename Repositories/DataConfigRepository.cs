using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IDataConfigRepository: IMongoRepository<DataConfig>
    {
        Task<IEnumerable<DataConfig>> GetAsync(string greenType, string type);
    }
    public class DataConfigRepository: MongoRepository<DataConfig>, IDataConfigRepository, IScopedLifetime
    {
        public DataConfigRepository(IMongoDbConnection mongoDbConnection): base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<DataConfig>> GetAsync(string greenType, string type)
        {
            var filter = Builders<DataConfig>.Filter.Empty;
            if (!string.IsNullOrEmpty(greenType))
            {
                filter &= Builders<DataConfig>.Filter.Regex(x => x.GreenType, new BsonRegularExpression($"/^{greenType}$/i"));
            }
            if (!string.IsNullOrEmpty(type))
            {
                filter &= Builders<DataConfig>.Filter.Regex(x => x.Type, new BsonRegularExpression($"/^{type}$/i"));
            }

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
