using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.MAFC
{
    public interface IMAFCUpdateInfoRepository : IMongoRepository<MAFCUpdateInfoModel>
    {
        Task<MAFCUpdateInfoModel> GetByCustomerIdAsync(string customerId);
    }
    public class MAFCUpdateInfoRepository : MongoRepository<MAFCUpdateInfoModel>, IMAFCUpdateInfoRepository, IScopedLifetime
    {
        public MAFCUpdateInfoRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<MAFCUpdateInfoModel> GetByCustomerIdAsync(string customerId)
        {
            var listRecord = await _collection.Find(x => x.CustomerId == customerId && !x.IsDelete).ToListAsync();
            return listRecord.LastOrDefault();
        }

    }
}
