using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.MCDebts;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IMCDebtRepository : IMongoRepository<MCDebt>
    {
        Task<IEnumerable<GetMCDebtResponse>> GetAsync(MCDebtStatus? mCDebtStatus, string textSearch, IEnumerable<string> creatorIds, int pageIndex, int pageSize);
        Task<long> CountAsync(MCDebtStatus? mCDebtStatus, string textSearch, IEnumerable<string> creatorIds);
        Task<GetDetailMCDebtResponse> GetDetailAsync(string id);
    }
    public class MCDebtRepository : MongoRepository<MCDebt>, IMCDebtRepository, IScopedLifetime
    {
        public MCDebtRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {

        }

        public async Task<IEnumerable<GetMCDebtResponse>> GetAsync(MCDebtStatus? mCDebtStatus, string textSearch, IEnumerable<string> creatorIds, int pageIndex, int pageSize)
        {
            var filter = GetFilter(mCDebtStatus, textSearch, creatorIds);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "AppNumber", 1 },
                    { "CustomerName", 1 },
                    { "CustomerPhone", "$Phone"},
                    { "CustomerIdCard", "$CitizenId"},
                    { "LoanApprovedAmt", 1 },
                    { "LoanApprovedTenor", 1 },
                    { "ContractNumber", 1 },
                    { "LoanCategory", 1},
                    { "LoanProduct", 1 },
                    { "HasInsurrance", 1 },
                    { "HasCourier", 1 },
                    { "AtBank", 1 },
                    { "AccountNumber", 1 },
                    { "AccountStatus", 1 },
                    { "DisbursementChannel", 1 },
                    { "ChannelName", 1 },
                    { "DisbursementDate", 1 },
                    { "CurrentDebtPeriod", 1 },
                    { "NextPaymentDate", 1 },
                    { "IsFollowed", 1 },
                    { "CreatedDate", 1 },
                    { "SaleInfo._id", 1},
                    { "SaleInfo.FullName", 1},
                    { "SaleInfo.UserName", 1}
                };

            IEnumerable<GetMCDebtResponse> mCDebts = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .Lookup("Users", "Creator", "_id", "SaleInfo")
                    .Project(projectMapping)
                    .As<GetMCDebtResponse>()
                    .ToListAsync();

            return mCDebts;
        }

        public async Task<long> CountAsync(MCDebtStatus? mCDebtStatus, string textSearch, IEnumerable<string> creatorIds)
        {
            var filter = GetFilter(mCDebtStatus, textSearch, creatorIds);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<MCDebt> GetFilter(MCDebtStatus? mCDebtStatus, string textSearch, IEnumerable<string> creatorIds)
        {
            var filter = Builders<MCDebt>.Filter.Eq(x => x.IsDeleted, false);

            if (!string.IsNullOrEmpty(textSearch))
            {
                filter &= Builders<MCDebt>.Filter.Or(
                 Builders<MCDebt>.Filter.Regex(x => x.CustomerName, new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i")),
                 Builders<MCDebt>.Filter.Regex(x => x.ContractCode, new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i")),
                 Builders<MCDebt>.Filter.Regex(x => x.CitizenId, new BsonRegularExpression($"/{textSearch.ConvertSpecialCharacters()}/i"))
                );
            }
            if (mCDebtStatus == MCDebtStatus.Paid)
            {
                filter &= Builders<MCDebt>.Filter.Gt(y => y.NextPaymentDate, DateTime.Now.AddDays(10));
            }
            else if (mCDebtStatus == MCDebtStatus.UnPaid)
            {
                filter &= Builders<MCDebt>.Filter.Lt(y => y.NextPaymentDate, DateTime.Now.AddDays(10));
            }
            if (mCDebtStatus == MCDebtStatus.UnFollow)
            {
                filter &= Builders<MCDebt>.Filter.Eq(y => y.IsFollowed, false);
            }
            else
            {
                filter &= Builders<MCDebt>.Filter.Eq(y => y.IsFollowed, true);
            }
            if (creatorIds.Any())
            {
                filter &= Builders<MCDebt>.Filter.In(x => x.Creator, creatorIds);
            }

            return filter;
        }

        public async Task<GetDetailMCDebtResponse> GetDetailAsync(string id)
        {
            var filter = Builders<MCDebt>.Filter.Eq(u => u.Id, id);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "AppNumber", 1 },
                    { "CustomerName", 1 },
                    { "CustomerPhone", "$Phone"},
                    { "CustomerIdCard", "$CitizenId"},
                    { "LoanApprovedAmt", 1 },
                    { "LoanApprovedTenor", 1 },
                    { "ContractCode", 1 },
                    { "ContractNumber", 1 },
                    { "LoanCategory", 1},
                    { "LoanProduct", 1 },
                    { "HasInsurrance", 1 },
                    { "HasCourier", 1 },
                    { "AtBank", 1 },
                    { "AccountNumber", 1 },
                    { "AccountStatus", 1 },
                    { "DisbursementChannel", 1 },
                    { "ChannelName", 1 },
                    { "DisbursementDate", 1 },
                    { "CurrentDebtPeriod", 1 },
                    { "NextPaymentDate", 1 },
                    { "TotalLoanAmt", 1 },
                    { "MonthlyPayment", 1 },
                    { "IsFollowed", 1 },
                    { "CreatedDate", 1 },
                    { "SaleInfo._id", 1},
                    { "SaleInfo.FullName", 1},
                    { "SaleInfo.UserName", 1}
                };
            GetDetailMCDebtResponse mcDebt = await _collection
                    .Aggregate()
                    .Match(filter)
                    .Lookup("Users", "Creator", "_id", "SaleInfo")
                    .Project(projectMapping)
                    .As<GetDetailMCDebtResponse>()
                    .FirstOrDefaultAsync();

            return mcDebt;
        }
    }
}
