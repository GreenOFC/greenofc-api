using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.PtfOmnis;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IPtfOmniMasterDataRepository : IMongoRepository<PtfOmniMasterData>
    {
        Task<PtfOmniMasterData> FindAsync(string type, string metaDataCode);
        Task<PtfOmniMasterData> FindMetadataAsync(string type, string key, int value);
    }
    public class PtfOmniMasterDataRepository : MongoRepository<PtfOmniMasterData>, IPtfOmniMasterDataRepository, IScopedLifetime
    {
        public PtfOmniMasterDataRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<PtfOmniMasterData> FindAsync(string type, string metaDataCode)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(nameof(PtfOmniMasterData.Type), type);
            filter &= Builders<BsonDocument>.Filter.Eq($"{nameof(PtfOmniMasterData.MetaData)}.code", metaDataCode);

            return await _collection
                .Aggregate()
                .As<BsonDocument>()
                .Match(filter)
                .As<PtfOmniMasterData>()
                .FirstOrDefaultAsync();
        }
        public async Task<PtfOmniMasterData> FindMetadataAsync(string type, string key, int value)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(nameof(PtfOmniMasterData.Type), type);
            filter &= Builders<BsonDocument>.Filter.Eq($"{nameof(PtfOmniMasterData.MetaData)}.{key}", value);
            filter &= Builders<BsonDocument>.Filter.Eq($"{nameof(PtfOmniMasterData.MetaData)}.allowed_channel", "3P");

            return await _collection
                .Aggregate()
                .As<BsonDocument>()
                .Match(filter)
                .As<PtfOmniMasterData>()
                .FirstOrDefaultAsync();
        }
    }
}
