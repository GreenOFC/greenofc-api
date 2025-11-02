using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ICounterRepository: IMongoRepository<Counter>
    {
        Task<int> GetNextSequenceAsync(string collectionName, int seq = 1);
    }
    public class CounterRepository: MongoRepository<Counter>, ICounterRepository, IScopedLifetime
    {
        public CounterRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {

        }

        public async Task<int> GetNextSequenceAsync(string collectionName, int seq = 1)
        {
            var update = new UpdateDefinitionBuilder<Counter>().Inc(n => n.Seq, 1);
            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                Projection = new ProjectionDefinitionBuilder<Counter>().Include(n => n.Seq)
            };
            var result = await _collection.FindOneAndUpdateAsync<Counter>(n => n.Id == collectionName, update, options);
            if(result == null)
            {
                result = new Counter { Id = collectionName, Seq = seq };
                await _collection.InsertOneAsync(result);
            }
            return result.Seq;
        }
    }
}
