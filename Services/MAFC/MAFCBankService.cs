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
    public interface IMAFCBankService
    {
        Task<IEnumerable<MAFCBankResponse>> GetAsync();
        Task SyncAsync();
    }
    public class MAFCBankService: IMAFCBankService, IScopedLifetime
    {
        private readonly ILogger<MAFCBankService> _logger;
        private readonly IRestMAFCMasterDataService _restMAFCMasterDataService;
        private readonly IMongoCollection<MAFCBank> _bankCollection;
        private readonly IMapper _mapper;

        public MAFCBankService(
            ILogger<MAFCBankService> logger,
            IRestMAFCMasterDataService restMAFCMasterDataService,
            IMongoDbConnection connection,
            IMapper mapper)
        {
            _logger = logger;
            _restMAFCMasterDataService = restMAFCMasterDataService;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _bankCollection = database.GetCollection<MAFCBank>(Common.MongoCollection.MAFCBank);
            _mapper = mapper;
        }

        public async Task<IEnumerable<MAFCBankResponse>> GetAsync()
        {
            try
            {
                var banks = await _bankCollection.Find(x => true).ToListAsync();
                var bankDtos = _mapper.Map<IEnumerable<MAFCBankResponse>>(banks);
                return bankDtos;
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
                var request = new MAFCMasterDataRequest { MsgName = MAFCMasterDataMessage.Bank };
                var result = await _restMAFCMasterDataService.GetAsync<IEnumerable<MAFCBankDto>>(request);
                await UpdateBankAsync(result.Data);
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

        private async Task UpdateBankAsync(IEnumerable<MAFCBankDto> banks)
        {
            var bankInDb = await _bankCollection.Find(x => true).ToListAsync();

            var bankToInsert = banks
                .Where(x => !bankInDb.Any(y => y.BankId == x.BankId))
                .Select(x => _mapper.Map<MAFCBank>(x));

            var bankToDelete = bankInDb.Where(x => !banks.Any(y => y.BankId == x.BankId));

            var bankToUpdate = bankInDb
                .Where(x => banks.Any(y => y.BankId == x.BankId))
                .Select(x =>
                {
                    var bank = banks.First(y => y.BankId == x.BankId);
                    _mapper.Map(bank, x);
                    return x;
                });

            if(bankToInsert.Any())
            {
                await InsertManyAsync(bankToInsert);
            }
            if (bankToDelete.Any())
            {
                await DeleteManyAsync(bankToDelete.Select(x => x.Id));
            }
            if (bankToUpdate.Any())
            {
                await UpdateManyAsync(bankToUpdate);
            }
        }

        private async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _bankCollection.DeleteManyAsync(x => ids.Contains(x.Id));
        }

        private async Task InsertManyAsync(IEnumerable<MAFCBank> banks)
        {
            await _bankCollection.InsertManyAsync(banks);
        }

        private async Task UpdateManyAsync(IEnumerable<MAFCBank> banks)
        {
            var listOfReplaceOneModels = banks.Select(bank => new ReplaceOneModel<MAFCBank>(Builders<MAFCBank>.Filter.Where(y => y.Id == bank.Id), bank));
            await _bankCollection.BulkWriteAsync(listOfReplaceOneModels);
        }
    }
}
