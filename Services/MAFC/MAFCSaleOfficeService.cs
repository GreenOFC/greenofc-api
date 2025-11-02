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
    public interface IMAFCSaleOfficeService
    {
        Task<IEnumerable<MAFCSaleOfficeResponse>> GetAsync();
        Task<MAFCSaleOfficeResponse> GetByMAFCCodeAsync(string mafcCode);
        Task SyncAsync();

    }
    public class MAFCSaleOfficeService : IMAFCSaleOfficeService, IScopedLifetime
    {
        private readonly ILogger<MAFCSaleOfficeService> _logger;
        private readonly IRestMAFCMasterDataService _restMAFCMasterDataService;
        private readonly IMongoCollection<MAFCSaleOffice> _saleOfficeCollection;
        private readonly IMapper _mapper;

        public MAFCSaleOfficeService(
            ILogger<MAFCSaleOfficeService> logger,
            IRestMAFCMasterDataService restMAFCMasterDataService,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            _restMAFCMasterDataService = restMAFCMasterDataService;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _saleOfficeCollection = database.GetCollection<MAFCSaleOffice>(Common.MongoCollection.MAFCSaleOffice);
            _mapper = mapper;
        }

        public async Task<IEnumerable<MAFCSaleOfficeResponse>> GetAsync()
        {
            try
            {
                var saleOffices = await _saleOfficeCollection.Find(x => true).ToListAsync();
                var saleOfficeDtos = _mapper.Map<IEnumerable<MAFCSaleOfficeResponse>>(saleOffices);
                return saleOfficeDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MAFCSaleOfficeResponse> GetByMAFCCodeAsync(string mafcCode)
        {
            try
            {
                var saleOffice = await _saleOfficeCollection.Find(x => x.InspectorName == mafcCode).FirstOrDefaultAsync();
                var saleOfficeDto = _mapper.Map<MAFCSaleOfficeResponse>(saleOffice);
                return saleOfficeDto;
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
                var request = new MAFCMasterDataRequest { MsgName = MAFCMasterDataMessage.SaleOffice };
                var result = await _restMAFCMasterDataService.GetAsync<IEnumerable<MAFCSaleOfficeDto>>(request);
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

        private async Task UpdateManyAsync(IEnumerable<MAFCSaleOfficeDto> saleOffices)
        {
            var saleOfficeInDb = await _saleOfficeCollection.Find(x => true).ToListAsync();

            var saleOfficeToInsert = saleOffices
                .Where(x => !saleOfficeInDb.Any(y => y.InspectorId == x.InspectorId))
                .Select(x => _mapper.Map<MAFCSaleOffice>(x));

            var saleOfficeToDelete = saleOfficeInDb.Where(x => !saleOffices.Any(y => y.InspectorId == x.InspectorId));

            var saleOfficeToUpdate = saleOfficeInDb
                .Where(x => saleOffices.Any(y => y.InspectorId == x.InspectorId))
                .Select(x =>
                {
                    var saleOffice = saleOffices.First(y => y.InspectorId == x.InspectorId);
                    _mapper.Map(saleOffice, x);
                    return x;
                });

            if (saleOfficeToInsert.Any())
            {
                await InsertManyAsync(saleOfficeToInsert);
            }
            if (saleOfficeToDelete.Any())
            {
                await DeleteManyAsync(saleOfficeToDelete.Select(x => x.Id));
            }
            if (saleOfficeToUpdate.Any())
            {
                await UpdateManyAsync(saleOfficeToUpdate);
            }
        }

        private async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _saleOfficeCollection.DeleteManyAsync(x => ids.Contains(x.Id));
        }

        private async Task InsertManyAsync(IEnumerable<MAFCSaleOffice> saleOffices)
        {
            await _saleOfficeCollection.InsertManyAsync(saleOffices);
        }

        private async Task UpdateManyAsync(IEnumerable<MAFCSaleOffice> saleOffices)
        {
            var listOfReplaceOneModels = saleOffices.Select(saleOffice => new ReplaceOneModel<MAFCSaleOffice>(Builders<MAFCSaleOffice>.Filter.Where(x => x.Id == saleOffice.Id), saleOffice));
            await _saleOfficeCollection.BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}
