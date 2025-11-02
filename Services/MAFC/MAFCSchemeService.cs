using _24hplusdotnetcore.Common;
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
    public interface IMAFCSchemeService
    {
        Task<IEnumerable<MAFCSchemeResponse>> GetAsync(string group);
        Task<MAFCSchemeResponse> GetDetailAsync(string id);
        Task SyncAsync();
    }
    public class MAFCSchemeService : IMAFCSchemeService, IScopedLifetime
    {
        private readonly ILogger<MAFCSchemeService> _logger;
        private readonly IRestMAFCMasterDataService _restMAFCMasterDataService;
        private readonly IMongoCollection<MAFCScheme> _schemeCollection;
        private readonly IMapper _mapper;

        public MAFCSchemeService(
            ILogger<MAFCSchemeService> logger,
            IRestMAFCMasterDataService restMAFCMasterDataService,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            _restMAFCMasterDataService = restMAFCMasterDataService;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _schemeCollection = database.GetCollection<MAFCScheme>(Common.MongoCollection.MAFCScheme);
            _mapper = mapper;
        }

        public async Task<IEnumerable<MAFCSchemeResponse>> GetAsync(string group)
        {
            try
            {
                List<MAFCScheme> schemes;
                if (string.IsNullOrEmpty(group))
                {
                    schemes = await _schemeCollection.Find(x => true).ToListAsync();
                }
                else
                {
                    schemes = await _schemeCollection.Find(x => true && x.SchemeGroup == group).ToListAsync();
                }
                var schemeDtos = _mapper.Map<IEnumerable<MAFCSchemeResponse>>(schemes);
                return schemeDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MAFCSchemeResponse> GetDetailAsync(string id)
        {
            try
            {
                var scheme = await _schemeCollection.FindSync(x => x.SchemeId == id).FirstOrDefaultAsync();
                if (scheme == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(MAFCScheme)));
                }
                return _mapper.Map<MAFCSchemeResponse>(scheme);
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
                var request = new MAFCMasterDataRequest { MsgName = MAFCMasterDataMessage.Scheme };
                var result = await _restMAFCMasterDataService.GetAsync<IEnumerable<MAFCSchemeDto>>(request);
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

        private async Task UpdateManyAsync(IEnumerable<MAFCSchemeDto> schemes)
        {
            var schemeInDb = await _schemeCollection.Find(x => true).ToListAsync();

            var schemeToInsert = schemes
                .Where(x => !schemeInDb.Any(y => y.SchemeId == x.SchemeId))
                .Select(x => _mapper.Map<MAFCScheme>(x));

            var schemeToDelete = schemeInDb.Where(x => !schemes.Any(y => y.SchemeId == x.SchemeId));

            var schemeToUpdate = schemeInDb
                .Where(x => schemes.Any(y => y.SchemeId == x.SchemeId))
                .Select(x =>
                {
                    var scheme = schemes.First(y => y.SchemeId == x.SchemeId);
                    _mapper.Map(scheme, x);
                    return x;
                });

            if (schemeToInsert.Any())
            {
                await InsertManyAsync(schemeToInsert);
            }
            if (schemeToDelete.Any())
            {
                await DeleteManyAsync(schemeToDelete.Select(x => x.Id));
            }
            if (schemeToUpdate.Any())
            {
                await UpdateManyAsync(schemeToUpdate);
            }
        }

        private async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _schemeCollection.DeleteManyAsync(x => ids.Contains(x.Id));
        }

        private async Task InsertManyAsync(IEnumerable<MAFCScheme> schemes)
        {
            await _schemeCollection.InsertManyAsync(schemes);
        }

        private async Task UpdateManyAsync(IEnumerable<MAFCScheme> schemes)
        {
            var listOfReplaceOneModels = schemes.Select(scheme => new ReplaceOneModel<MAFCScheme>(Builders<MAFCScheme>.Filter.Where(x => x.Id == scheme.Id), scheme));
            await _schemeCollection.BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}
