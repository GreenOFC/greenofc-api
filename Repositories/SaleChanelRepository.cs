using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ISaleChanelRepository : IMongoRepository<SaleChanel>
    {
        Task<IEnumerable<SaleChanelResponse>> GetAsync(string textSearch, int pageIndex, int pageSize);

        Task<long> CountAsync(string textSearch);

        Task<SaleChanelDetailResponse> GetDetailAsync(string id);

        Task AddPosAsync(string id, SaleChanelPosInfo posInfo);

        Task DeletePosAsync(string id, string posId);

        Task UpdateSaleChanelConfigUserInfoAsync(string saleChanelConfigUserId, SaleChanelConfigUserInfo saleChanelConfigUserInfo);
    }

    public class SaleChanelRepository : MongoRepository<SaleChanel>, ISaleChanelRepository, IScopedLifetime
    {
        private readonly ILogger<SaleChanelRepository> _logger;
        private readonly IUserLoginService _userLoginService;

        public SaleChanelRepository(
            ILogger<SaleChanelRepository> logger, 
            IMongoDbConnection mongoDbConnection,
            IUserLoginService userLoginService) : base(mongoDbConnection)
        {
            _logger = logger;
            _userLoginService = userLoginService;
        }

        public async Task<IEnumerable<SaleChanelResponse>> GetAsync(string textSearch, int pageIndex, int pageSize)
        {
            var filter = GetFilter(textSearch);

            return await _collection
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .As<SaleChanelResponse>()
                .ToListAsync();
        }

        public async Task<long> CountAsync(string textSearch)
        {
            var filter = GetFilter(textSearch);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        public async Task<SaleChanelDetailResponse> GetDetailAsync(string id)
        {
            return await _collection
                .Aggregate()
                .Match(x => x.Id == id && x.IsDeleted != true)
                .As<SaleChanelDetailResponse>()
                .FirstOrDefaultAsync();
        }

        public async Task AddPosAsync(string id, SaleChanelPosInfo posInfo)
        {
            var saleChanel = await FindByIdAsync(id);
            if (saleChanel == null)
            {
                throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
            }
            var poses = saleChanel.Poses?.ToList() ?? new List<SaleChanelPosInfo>();
            if (poses.Any(x => x.Id == posInfo.Id)) return;

            poses.Add(posInfo);
            var update = Builders<SaleChanel>.Update
                .Set(x => x.Poses, poses)
                .Set(x => x.ModifiedDate, DateTime.Now)
                .Set(x => x.Modifier, _userLoginService.GetUserId());

            await UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task DeletePosAsync(string id, string posId)
        {
            var saleChanel = await FindByIdAsync(id);
            if (saleChanel == null)
            {
                throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(SaleChanel)));
            }

            var poses = saleChanel.Poses?.ToList() ?? new List<SaleChanelPosInfo>();
            if (!poses.Any(x => x.Id == posId)) return;

            var newPoses = poses.Where(x => x.Id != posId);
            var update = Builders<SaleChanel>.Update
                .Set(x => x.Poses, newPoses)
                .Set(x => x.ModifiedDate, DateTime.Now)
                .Set(x => x.Modifier, _userLoginService.GetUserId());
            await UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task UpdateSaleChanelConfigUserInfoAsync(string saleChanelConfigUserId, SaleChanelConfigUserInfo saleChanelConfigUserInfo)
        {
            var update = Builders<SaleChanel>.Update
                .Set(x => x.SaleChanelConfigUserInfo, saleChanelConfigUserInfo)
                .Set(x => x.ModifiedDate, DateTime.Now)
                .Set(x => x.Modifier, _userLoginService.GetUserId());
            await UpdateManyAsync(x => x.SaleChanelConfigUserId == saleChanelConfigUserId, update);
        }

        private FilterDefinition<SaleChanel> GetFilter(string textSearch)
        {
            var filter = Builders<SaleChanel>.Filter.Ne(x => x.IsDeleted, true);

            if (!string.IsNullOrEmpty(textSearch))
            {
                var regex = new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<SaleChanel>.Filter.Regex(x => x.Code, regex) |
                    Builders<SaleChanel>.Filter.Regex(x => x.Name, regex);
            }

            return filter;
        }

    }
}
