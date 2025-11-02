using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMAFCDistrictService
    {
        Task<IEnumerable<MAFCDistrictResponse>> GetAsync(string stateId);
        Task SyncAsync();
    }
    public class MAFCDistrictService : IMAFCDistrictService, IScopedLifetime
    {
        private readonly ILogger<MAFCDistrictService> _logger;
        private readonly IRestMAFCMasterDataService _restMAFCMasterDataService;
        private readonly IMongoCollection<MAFCDistrict> _districtCollection;
        private readonly IMapper _mapper;

        public MAFCDistrictService(
            ILogger<MAFCDistrictService> logger,
            IRestMAFCMasterDataService restMAFCMasterDataService,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            _restMAFCMasterDataService = restMAFCMasterDataService;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _districtCollection = database.GetCollection<MAFCDistrict>(Common.MongoCollection.MAFCDistrict);
            _mapper = mapper;
        }

        public async Task<IEnumerable<MAFCDistrictResponse>> GetAsync(string stateId)
        {
            try
            {
                Expression<Func<MAFCDistrict, bool>> filter = x => true;
                if (!string.IsNullOrEmpty(stateId))
                {
                    filter = district => district.StateId == stateId;
                }
                var districts = await _districtCollection.Find(filter).ToListAsync();
                var response = _mapper.Map<IEnumerable<MAFCDistrictResponse>>(districts);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SyncAsync()
        {
            try
            {
                var request = new MAFCMasterDataRequest { MsgName = MAFCMasterDataMessage.District };
                var result = await _restMAFCMasterDataService.GetAsync<IEnumerable<MAFCDistrictDto>>(request);
                await UpdateManyAsync(result.Data);
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task UpdateManyAsync(IEnumerable<MAFCDistrictDto> districts)
        {
            var districtInDb = await _districtCollection.Find(x => true).ToListAsync();

            var districtToInsert = districts
                .Where(x => !districtInDb.Any(y => y.CityId == x.CityId))
                .Select(x => _mapper.Map<MAFCDistrict>(x));

            var districtToDelete = districtInDb.Where(x => !districts.Any(y => y.CityId == x.CityId));

            var districtToUpdate = districtInDb
                .Where(x => districts.Any(y => y.CityId == x.CityId))
                .Select(x =>
                {
                    var district = districts.First(y => y.CityId == x.CityId);
                    _mapper.Map(district, x);
                    return x;
                });

            if (districtToInsert.Any())
            {
                await InsertManyAsync(districtToInsert);
            }
            if (districtToDelete.Any())
            {
                await DeleteManyAsync(districtToDelete.Select(x => x.Id));
            }
            if (districtToUpdate.Any())
            {
                await UpdateManyAsync(districtToUpdate);
            }
        }

        private async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _districtCollection.DeleteManyAsync(x => ids.Contains(x.Id));
        }

        private async Task InsertManyAsync(IEnumerable<MAFCDistrict> districts)
        {
            await _districtCollection.InsertManyAsync(districts);
        }

        private async Task UpdateManyAsync(IEnumerable<MAFCDistrict> districts)
        {
            var listOfReplaceOneModels = districts.Select(district => new ReplaceOneModel<MAFCDistrict>(Builders<MAFCDistrict>.Filter.Where(x => x.Id == district.Id), district));
            await _districtCollection.BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}
