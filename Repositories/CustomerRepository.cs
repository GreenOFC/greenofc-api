using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.File;
using _24hplusdotnetcore.ModelDtos.LeadCimbs;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ICustomerRepository : IMongoRepository<Customer>
    {
        Task<long> CountReturnAsync(string username);
        Task<IEnumerable<T>> GetAsync<T>(CustonerFilterDto custonerFilterDto);
        Task<long> CountAsync(CustonerFilterDto custonerFilterDto);
        Task<IEnumerable<ExportCimbModel>> GetExportAsync(string greenType, IEnumerable<string> creatorIds);

    }
    public class CustomerRepository : MongoRepository<Customer>, ICustomerRepository, IScopedLifetime
    {
        public CustomerRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {
        }

        public async Task<long> CountReturnAsync(string username)
        {
            var filter = Builders<Customer>.Filter.And(
                Builders<Customer>.Filter.Ne(c => c.IsDeleted, true),
                Builders<Customer>.Filter.Eq(c => c.UserName, username),
                Builders<Customer>.Filter.Eq(c => c.Status, CustomerStatus.RETURN)
            );
            return await _collection.Find(filter).CountDocumentsAsync();
        }

        public async Task<IEnumerable<T>> GetAsync<T>(CustonerFilterDto custonerFilterDto)
        {
            var filter = GetFilter(custonerFilterDto);
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            IEnumerable<T> listOfNews = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Skip((custonerFilterDto.PageIndex - 1) * custonerFilterDto.PageSize)
                    .Limit(custonerFilterDto.PageSize)
                    .As<T>()
                    .ToListAsync();

            return listOfNews;
        }

        public async Task<IEnumerable<ExportCimbModel>> GetExportAsync(string greenType, IEnumerable<string> creatorIds)
        {
            var filter = GetFilter(new CustonerFilterDto { GreenType = greenType, CreatorIds = creatorIds });
            var unwindOption = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };
            var projectMapping = new BsonDocument()
                {
                    { "SalesName", "$CreatorUser.FullName"},
                    { "ModifiedTime", "$ModifiedDate"},
                    { "CustomerName", "$Personal.Name"},
                };
            IEnumerable<ExportCimbModel> listOfNews = await _collection
                    .Aggregate()
                    .Match(filter)
                    .SortByDescending(c => c.ModifiedDate)
                    .Lookup("Users", "Creator", "_id", "CreatorUser")
                    .Unwind("CreatorUser", unwindOption)
                    .Project(projectMapping)
                    .As<ExportCimbModel>()
                    .ToListAsync();

            return listOfNews;
        }

        public async Task<long> CountAsync(CustonerFilterDto custonerFilterDto)
        {
            var filter = GetFilter(custonerFilterDto);
            var total = await _collection.Find(filter).CountDocumentsAsync();
            return total;
        }

        private FilterDefinition<Customer> GetFilter(CustonerFilterDto custonerFilterDto)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.IsDeleted, false);
            filter &= Builders<Customer>.Filter.Eq(x => x.GreenType, custonerFilterDto.GreenType);
            if (custonerFilterDto.CreatorIds.Any())
            {
                filter &= Builders<Customer>.Filter.In(x => x.Creator, custonerFilterDto.CreatorIds);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.Status))
            {
                filter &= Builders<Customer>.Filter.Eq(x => x.Status, custonerFilterDto.Status);
            }
            if (custonerFilterDto.HasMafcId)
            {
                filter &= Builders<Customer>.Filter.Ne(x => x.MAFCId, 0);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.CustomerName))
            {
                var regex = new BsonRegularExpression($"/{custonerFilterDto.CustomerName.ConvertSpecialCharacters()}/i");
                filter &= Builders<Customer>.Filter.Regex(x => x.Personal.Name, regex) |
                    Builders<Customer>.Filter.Regex(x => x.Personal.IdCard, regex) |
                    Builders<Customer>.Filter.Regex(x => x.ContractCode, regex);
            }
            if (custonerFilterDto.FromDate.HasValue)
            {
                filter &= Builders<Customer>.Filter.Gte(x => x.ModifiedDate, custonerFilterDto.FromDate);
            }
            if (custonerFilterDto.ToDate.HasValue)
            {
                filter &= Builders<Customer>.Filter.Lte(x => x.ModifiedDate, custonerFilterDto.ToDate);
            }
            if (custonerFilterDto.FromCreateDate.HasValue)
            {
                filter &= Builders<Customer>.Filter.Gte(x => x.CreatedDate, custonerFilterDto.FromCreateDate);
            }
            if (custonerFilterDto.ToCreateDate.HasValue)
            {
                filter &= Builders<Customer>.Filter.Lte(x => x.CreatedDate, custonerFilterDto.ToCreateDate);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.ProductLine))
            {
                filter &= Builders<Customer>.Filter.Eq(x => x.ProductLine, custonerFilterDto.ProductLine);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.ReturnStatus))
            {
                var regex = new BsonRegularExpression($"/{custonerFilterDto.ReturnStatus.ConvertSpecialCharacters()}/i");
                filter &= Builders<Customer>.Filter.Regex(x => x.Result.ReturnStatus, regex);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.Sale))
            {
                var regex = new BsonRegularExpression($"/{custonerFilterDto.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<Customer>.Filter.Regex(x => x.SaleInfo.Name, regex) |
                    Builders<Customer>.Filter.Regex(x => x.SaleInfo.Code, regex);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{custonerFilterDto.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<Customer>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<Customer>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.Asm))
            {
                var regex = new BsonRegularExpression($"/{custonerFilterDto.Asm.ConvertSpecialCharacters()}/i");
                filter &= Builders<Customer>.Filter.Regex(x => x.AsmInfo.FullName, regex) |
                    Builders<Customer>.Filter.Regex(x => x.AsmInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(custonerFilterDto.PosManager))
            {
                var regex = new BsonRegularExpression($"/{custonerFilterDto.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<Customer>.Filter.Regex(x => x.PosInfo.Name, regex);
            }
            return filter;
        }
    }
}
