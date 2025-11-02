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
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMAFCCityService
    {
        Task<IEnumerable<MAFCCityResponse>> GetAsync();
        Task SyncAsync();
    }
    public class MAFCCityService : IMAFCCityService, IScopedLifetime
    {
        private readonly ILogger<MAFCCityService> _logger;
        private readonly IRestMAFCMasterDataService _restMAFCMasterDataService;
        private readonly IMongoCollection<MAFCCity> _cityCollection;
        private readonly IMapper _mapper;

        public MAFCCityService(
            ILogger<MAFCCityService> logger,
            IRestMAFCMasterDataService restMAFCMasterDataService,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            _restMAFCMasterDataService = restMAFCMasterDataService;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _cityCollection = database.GetCollection<MAFCCity>(Common.MongoCollection.MAFCCity);
            _mapper = mapper;
        }

        public async Task<IEnumerable<MAFCCityResponse>> GetAsync()
        {
            try
            {
                var cities = await _cityCollection.Find(x => true).ToListAsync();
                var response = _mapper.Map<IEnumerable<MAFCCityResponse>>(cities);
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
                var request = new MAFCMasterDataRequest { MsgName = MAFCMasterDataMessage.City };
                var result = await _restMAFCMasterDataService.GetAsync<IEnumerable<MAFCCityDto>>(request);
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

        private async Task UpdateManyAsync(IEnumerable<MAFCCityDto> cities)
        {
            var cityInDb = await _cityCollection.Find(x => true).ToListAsync();

            var cityToInsert = cities
                .Where(x => !cityInDb.Any(y => y.StateId == x.StateId))
                .Select(x => _mapper.Map<MAFCCity>(x));

            var cityToDelete = cityInDb.Where(x => !cities.Any(y => y.StateId == x.StateId));

            var cityToUpdate = cityInDb
                .Where(x => cities.Any(y => y.StateId == x.StateId))
                .Select(x =>
                {
                    var city = cities.First(y => y.StateId == x.StateId);
                    _mapper.Map(city, x);
                    return x;
                });

            if (cityToInsert.Any())
            {
                await InsertManyAsync(cityToInsert);
            }
            if (cityToDelete.Any())
            {
                await DeleteManyAsync(cityToDelete.Select(x => x.Id));
            }
            if (cityToUpdate.Any())
            {
                await UpdateManyAsync(cityToUpdate);
            }
        }

        private async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _cityCollection.DeleteManyAsync(x => ids.Contains(x.Id));
        }

        private async Task InsertManyAsync(IEnumerable<MAFCCity> cities)
        {
            await _cityCollection.InsertManyAsync(cities);
        }

        private async Task UpdateManyAsync(IEnumerable<MAFCCity> cities)
        {
            var listOfReplaceOneModels = cities.Select(city => new ReplaceOneModel<MAFCCity>(Builders<MAFCCity>.Filter.Where(x => x.Id == city.Id), city));
            await _cityCollection.BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}
