using _24hplusdotnetcore.ModelDtos.Banners;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IBannerRepository: IMongoRepository<Banner>
    {
        Task<IEnumerable<GetBannerResponse>> GetAsync(DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize);
        Task<long> CountAsync(DateTime? startDate, DateTime? endDate);
    }
    public class BannerRepository: MongoRepository<Banner>, IBannerRepository, IScopedLifetime
    {
        public BannerRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {

        }

        public async Task<IEnumerable<GetBannerResponse>> GetAsync(DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize)
        {
            var filter = GetFilter(startDate, endDate);
            IEnumerable<GetBannerResponse> banners = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(x => x.CreatedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .As<GetBannerResponse>()
                    .ToListAsync();

            return banners;
        }

        public async Task<long> CountAsync(DateTime? startDate, DateTime? endDate)
        {
            var filter = GetFilter(startDate, endDate);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<Banner> GetFilter(DateTime? startDate, DateTime? endDate)
        {
            var filter = Builders<Banner>.Filter.Eq(banner => banner.IsDeleted, false);
            if (startDate.HasValue)
            {
                filter &= Builders<Banner>.Filter.Gte(banner => banner.StartDate, startDate?.Date);
                filter &= Builders<Banner>.Filter.Lt(banner => banner.StartDate, startDate?.Date.AddDays(1));
            }
            if (endDate.HasValue)
            {
                filter &= Builders<Banner>.Filter.Gte(banner => banner.EndDate, endDate?.Date);
                filter &= Builders<Banner>.Filter.Lt(banner => banner.EndDate, endDate?.Date.AddDays(1));
            }
            return filter;
        }
    }
}
