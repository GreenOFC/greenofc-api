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
    public interface IMAFCWardService
    {
        Task<IEnumerable<MAFCWardResponse>> GetAsync(string cityId);
        Task SyncAsync();
    }
    public class MAFCWardService : IMAFCWardService, IScopedLifetime
    {
        private readonly ILogger<MAFCWardService> _logger;
        private readonly IRestMAFCMasterDataService _restMAFCMasterDataService;
        private readonly IMongoCollection<MAFCWard> _wardCollection;
        private readonly IMapper _mapper;

        public MAFCWardService(
            ILogger<MAFCWardService> logger,
            IRestMAFCMasterDataService restMAFCMasterDataService,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            _restMAFCMasterDataService = restMAFCMasterDataService;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _wardCollection = database.GetCollection<MAFCWard>(Common.MongoCollection.MAFCWard);
            _mapper = mapper;
        }

        public async Task<IEnumerable<MAFCWardResponse>> GetAsync(string cityId)
        {
            try
            {
                Expression<Func<MAFCWard, bool>> filter = x => true;
                if (!string.IsNullOrEmpty(cityId))
                {
                    filter = ward => ward.CityId == cityId;
                }
                var wards = await _wardCollection.Find(filter).ToListAsync();
                var response = _mapper.Map<IEnumerable<MAFCWardResponse>>(wards);
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
                var request = new MAFCMasterDataRequest { MsgName = MAFCMasterDataMessage.Ward };
                var result = await _restMAFCMasterDataService.GetAsync<IEnumerable<MAFCWardDto>>(request);
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

        private async Task UpdateManyAsync(IEnumerable<MAFCWardDto> wards)
        {
            var wardInDb = await _wardCollection.Find(x => true).ToListAsync();

            var wardToInsert = wards
                .Where(x => !wardInDb.Any(y => y.ZipCode == x.ZipCode))
                .Select(x => _mapper.Map<MAFCWard>(x));

            var wardToDelete = wardInDb.Where(x => !wards.Any(y => y.ZipCode == x.ZipCode));

            var wardToUpdate = wardInDb
                .Where(x => wards.Any(y => y.ZipCode == x.ZipCode))
                .Select(x =>
                {
                    var ward = wards.First(y => y.ZipCode == x.ZipCode);
                    _mapper.Map(ward, x);
                    return x;
                });

            if (wardToInsert.Any())
            {
                await InsertManyAsync(wardToInsert);
            }
            if (wardToDelete.Any())
            {
                await DeleteManyAsync(wardToDelete.Select(x => x.Id));
            }
            if (wardToUpdate.Any())
            {
                await UpdateManyAsync(wardToUpdate);
            }
        }

        private async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _wardCollection.DeleteManyAsync(x => ids.Contains(x.Id));
        }

        private async Task InsertManyAsync(IEnumerable<MAFCWard> wards)
        {
            await _wardCollection.InsertManyAsync(wards);
        }

        private async Task UpdateManyAsync(IEnumerable<MAFCWard> wards)
        {
            var listOfReplaceOneModels = wards.Select(ward => new ReplaceOneModel<MAFCWard>(Builders<MAFCWard>.Filter.Where(x => x.Id == ward.Id), ward));
            await _wardCollection.BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}
