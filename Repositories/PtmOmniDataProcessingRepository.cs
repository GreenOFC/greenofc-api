using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.PtfOmnis;
using _24hplusdotnetcore.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IPtmOmniDataProcessingRepository : IMongoRepository<PtfOmniDataProcessing>
    {
        Task<IEnumerable<PtfOmniGetDataProcessingResponse>> GetAsync(string customerId, int pageIndex, int pageSize);
        Task<long> CountAsync(string customerId);
    }
    public class PtmOmniDataProcessingRepository: MongoRepository<PtfOmniDataProcessing>, IPtmOmniDataProcessingRepository, IScopedLifetime
    {
        public PtmOmniDataProcessingRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<IEnumerable<PtfOmniGetDataProcessingResponse>> GetAsync(string customerId, int pageIndex, int pageSize)
        {
            var filter = GetFilter(customerId);
            return await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .As<PtfOmniGetDataProcessingResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(string customerId)
        {
            var filter = GetFilter(customerId);

            var total = await _collection
                .Find(filter)
                .CountDocumentsAsync();

            return total;
        }

        private FilterDefinition<PtfOmniDataProcessing> GetFilter(string customerId)
        {
            return Builders<PtfOmniDataProcessing>.Filter.Eq(x => x.CustomerId, customerId);
        }
    }
}
