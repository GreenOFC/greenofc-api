using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ISmsHistoryRepository: IMongoRepository<SmsHistory>
    {
        Task<long> CountAsync(string phoneNumber, DateTime minDate);
    }
    public class SmsHistoryRepository: MongoRepository<SmsHistory>, ISmsHistoryRepository, IScopedLifetime
    {
        public SmsHistoryRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<long> CountAsync(string phoneNumber, DateTime minDate)
        {
            var filter = Builders<SmsHistory>.Filter.And(
                Builders<SmsHistory>.Filter.Eq(c => c.PhoneNumber, phoneNumber),
                Builders<SmsHistory>.Filter.Eq(c => c.IsSuccess, true),
                Builders<SmsHistory>.Filter.Gt(c => c.CreatedDate, minDate)
            );
            return await _collection.Find(filter).CountDocumentsAsync();
        }
    }
}
