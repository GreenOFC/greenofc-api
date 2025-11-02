using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public interface IMCKiosService
    {
        Task SyncAsync();
        Task<IEnumerable<KiosModel>> GetAsync();
    }
    public class MCKiosService: IMCKiosService, IScopedLifetime
    {
        private readonly ILogger<MCKiosService> _logger;
        private readonly IRestMCService _restMCService;
        private readonly IMongoCollection<MCKios> _mckiosCollection;
        private readonly IMapper _mapper;

        public MCKiosService(
            ILogger<MCKiosService> logger, 
            IRestMCService restMCService,
            IMapper mapper,
            IMongoDbConnection connection)
        {
            _logger = logger;
            _restMCService = restMCService;
            _mapper = mapper;

            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _mckiosCollection = database.GetCollection<MCKios>(MongoCollection.MCKios);
        }

        public async Task<IEnumerable<KiosModel>> GetAsync()
        {
            try
            {
                var kios = await _mckiosCollection.Find(x => true).ToListAsync();
                var kiosDtos = _mapper.Map<IEnumerable<KiosModel>>(kios);
                return kiosDtos;
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
                IEnumerable<KiosModel> kios = await _restMCService.GetKiosAsync();
                await UpdateManyAsync(kios);
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

        private async Task UpdateManyAsync(IEnumerable<KiosModel> kios)
        {
            var kiosInDb = await _mckiosCollection.Find(x => true).ToListAsync();

            var kiosToInsert = kios
                .Where(x => !kiosInDb.Any(y => y.Id == x.Id))
                .Select(x => _mapper.Map<MCKios>(x));

            var kiosToDelete = kiosInDb.Where(x => !kios.Any(y => y.Id == x.Id));

            var kiosToUpdate = kiosInDb
                .Where(x => kios.Any(y => y.Id == x.Id))
                .Select(x =>
                {
                    var item = kios.First(y => y.Id == x.Id);
                    _mapper.Map(item, x);
                    x.UpdatedDateTime = DateTime.Now;
                    return x;
                });

            if (kiosToInsert.Any())
            {
                await InsertManyAsync(kiosToInsert);
            }
            if (kiosToDelete.Any())
            {
                await DeleteManyAsync(kiosToDelete.Select(x => x.Id));
            }
            if (kiosToUpdate.Any())
            {
                await UpdateManyAsync(kiosToUpdate);
            }
        }

        private async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _mckiosCollection.DeleteManyAsync(x => ids.Contains(x.Id));
        }

        private async Task InsertManyAsync(IEnumerable<MCKios> kios)
        {
            await _mckiosCollection.InsertManyAsync(kios);
        }

        private async Task UpdateManyAsync(IEnumerable<MCKios> kios)
        {
            var listOfReplaceOneModels = kios.Select(item => new ReplaceOneModel<MCKios>(Builders<MCKios>.Filter.Where(x => x.Id == item.Id), item));
            await _mckiosCollection.BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}
