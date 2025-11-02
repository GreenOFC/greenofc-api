using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.eWalletTransaction;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.eWalletTransaction
{
    public interface ITransactionRepository : IMongoRepository<TransactionModel>
    {
        Task<IEnumerable<TransactionResponse>> GetListTransactionAsync(IEnumerable<string> listCreators, TransactionRequest request);

        Task<long> CountAsync(IEnumerable<string> listCreators, TransactionRequest request);
        Task UpdateTransactionStatus(string id, string modifier, TransactionStatus status);
    }
    public class TransactionRepository : MongoRepository<TransactionModel>, ITransactionRepository, IScopedLifetime
    {
        private readonly ILogger<TransactionRepository> _logger;
        private readonly ICounterRepository _counterRepository;
        public TransactionRepository(
            IMongoDbConnection mongoDbConnection,
            ICounterRepository counterRepository,
            ILogger<TransactionRepository> logger) : base(mongoDbConnection)
        {
            _counterRepository = counterRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionResponse>> GetListTransactionAsync(IEnumerable<string> listCreators, TransactionRequest request)
        {
            try
            {
                var filter = GetFilter(listCreators, request);
                var TransactionLst = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(x => x.ModifiedDate)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Limit(request.PageSize)
                    .As<TransactionResponse>().ToListAsync();
                return TransactionLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> CountAsync(IEnumerable<string> listCreators, TransactionRequest request)
        {
            var filter = GetFilter(listCreators, request);

            var total = await _collection
                .Find(filter)
                .CountDocumentsAsync();

            return total;
        }

        private FilterDefinition<TransactionModel> GetFilter(IEnumerable<string> userIds, TransactionRequest request)
        {
            try
            {
                var filter = Builders<TransactionModel>.Filter.Empty;
                if (userIds?.Any() == true)
                {
                    filter &= Builders<TransactionModel>.Filter.In(x => x.Creator, userIds);
                }
                if (!string.IsNullOrEmpty(request.Status))
                {
                    Enum.TryParse<TransactionStatus>(request.Status, out TransactionStatus eStatus);
                    filter &= Builders<TransactionModel>.Filter.Eq(x => x.Status, eStatus);
                }
                if (!string.IsNullOrEmpty(request.BillStatus))
                {
                    Enum.TryParse<BillStatus>(request.BillStatus, out BillStatus bStatus);
                    filter &= Builders<TransactionModel>.Filter.Eq(x => x.BillStatus, bStatus);
                }
                if (!string.IsNullOrEmpty(request.Sale))
                {
                    filter &= Builders<TransactionModel>.Filter.Or(
                        Builders<TransactionModel>.Filter.Eq(x => x.SaleInfo.Code, request.Sale), 
                        Builders<TransactionModel>.Filter.Eq(x => x.SaleInfo.Name, request.Sale));
                }
                if (!string.IsNullOrEmpty(request.Type))
                {
                    filter &= Builders<TransactionModel>.Filter.Eq(x => x.Type, request.Type);
                }
                if (!string.IsNullOrEmpty(request.Amount))
                {
                    double.TryParse(request.Amount, out double qAmount);
                    filter &= Builders<TransactionModel>.Filter.Eq(x => x.Amount, qAmount);
                }
                if (!string.IsNullOrEmpty(request.PartnerTransaction))
                {
                    var regex = new BsonRegularExpression($"/{request.PartnerTransaction.ConvertSpecialCharacters()}/i");
                    filter &= Builders<TransactionModel>.Filter.Regex(x => x.PartnerTransaction, regex);
                }
                filter &= Builders<TransactionModel>.Filter.Gte(x => x.ModifiedDate, request.GetFromDate());
                filter &= Builders<TransactionModel>.Filter.Lte(x => x.ModifiedDate, request.GetToDate());
                return filter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateTransactionStatus(string id, string modifier, TransactionStatus status)
        {
            try
            {
                UpdateOptions updateOptions = new UpdateOptions
                {
                    IsUpsert = false
                };

                var update = Builders<TransactionModel>.Update
                     .Set(x => x.Status, status)
                     .Set(x => x.Modifier, modifier)
                     .Set(x => x.ModifiedDate, DateTime.Now);

                await _collection.UpdateOneAsync(x => x.Id == id, update, updateOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
