using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.F88;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ILeadF88Repository 
    {
        Task<LeadF88> GetDetail(string id);
        Task<LeadF88> GetDetailByIdCard(string idCard);
        Task<LeadF88> GetDetailByF88Id(string f88Id);
        Task<IEnumerable<LeadF88>> GetByIds(IEnumerable<string> ids);
        Task<GetLeadF88Response> GetRelatedDetailAsync(string id);
        Task<IEnumerable<GetLeadF88Response>> GetAsync(IEnumerable<string> creatorIds, GetLeadF88Request getLeadF88Request);

        Task<long> CountAsync(IEnumerable<string> creatorIds, GetLeadF88Request getLeadF88Request);
        Task CreateAsync(LeadF88 request);
        Task ReplaceOneAsync(LeadF88 request);
        Task<IEnumerable<LeadF88>> GetByIdCardAsync(string idCard);
    }
    public class LeadF88Repository : ILeadF88Repository, IScopedLifetime
    {
        private readonly ILogger<LeadF88Repository> _logger;
        private readonly IMongoRepository<LeadSource> _leadsourceRepository;
        private readonly IMongoRepository<LeadF88> _leadF88Repository;

        public LeadF88Repository(
            ILogger<LeadF88Repository> logger,
            IMongoRepository<LeadSource> leadsourceRepository,
            IMongoRepository<LeadF88> leadF88Repository
         )
        {
            _logger = logger;
            _leadsourceRepository = leadsourceRepository;
            _leadF88Repository = leadF88Repository;
        }

        public async Task<IEnumerable<GetLeadF88Response>> GetAsync(IEnumerable<string> creatorIds, GetLeadF88Request getLeadF88Request)
        {
            var filter = GetFilter(creatorIds, getLeadF88Request);

            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "Name", 1 },
                    { "ContractCode", 1 },
                    { "Phone", 1 },
                    { "IdCard", 1 },
                    { "IdCardProvince", 1 },
                    { "Gender", 1 },
                    { "Address", 1 },
                    { "DateOfBirth", 1 },
                    { "Description", 1 },
                    { "Status", 1 },

                    { "LoanCategoryData", 1 },
                    { "ProvinceData", 1 },
                    { "SignAddressData", 1 },
                    { "PostBack", 1 },

                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "SaleInfo", "$SaleInfomation"},
                    { "PosInfo", 1},
                    { "TeamLeadInfo", 1},
                    { "SaleChanelInfo", 1},
                };
            return await _leadsourceRepository.GetCollection().OfType<LeadF88>().Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((getLeadF88Request.PageIndex - 1) * getLeadF88Request.PageSize)
                .Limit(getLeadF88Request.PageSize)
                .Project(projectMapping)
                .As<GetLeadF88Response>()
                .ToListAsync();
        }

        public async Task<GetLeadF88Response> GetRelatedDetailAsync(string id)
        {
            var filter = Builders<LeadF88>.Filter.Eq(x => x.Id, id);

            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "Name", 1 },
                    { "ContractCode", 1 },
                    { "Phone", 1 },
                    { "IdCard", 1 },
                    { "IdCardProvince", 1 },
                    { "Gender", 1 },
                    { "Address", 1 },
                    { "DateOfBirth", 1 },
                    { "Description", 1 },
                    { "Status", 1 },

                    { "LoanCategoryData", 1 },
                    { "ProvinceData", 1 },
                    { "SignAddressData", 1 },
                    { "PostBack", 1 },

                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "SaleInfo", "$SaleInfomation"},
                };

            return await _leadsourceRepository.GetCollection().OfType<LeadF88>().Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Project(projectMapping)
                .As<GetLeadF88Response>()
                .FirstOrDefaultAsync();
        }

        public async Task<long> CountAsync(IEnumerable<string> creatorIds, GetLeadF88Request getLeadF88Request)
        {
            var filter = GetFilter(creatorIds, getLeadF88Request);

            var total = await _leadsourceRepository.GetCollection().OfType<LeadF88>().Find(filter).CountDocumentsAsync();

            return total;
        }

        private FilterDefinition<LeadF88> GetFilter(IEnumerable<string> creatorIds, GetLeadF88Request getLeadF88Request)
        {
            var filter = Builders<LeadF88>.Filter.Empty;

            if (!string.IsNullOrEmpty(getLeadF88Request.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{getLeadF88Request.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadF88>.Filter.Regex(x => x.Name, regex) |
                    Builders<LeadF88>.Filter.Regex(x => x.IdCard, regex) |
                    Builders<LeadF88>.Filter.Regex(x => x.ContractCode, regex);
            }
            if (creatorIds.Any())
            {
                filter &= Builders<LeadF88>.Filter.In(x => x.Creator, creatorIds);
            }
            if (!string.IsNullOrEmpty(getLeadF88Request.Sale))
            {
                var regex = new BsonRegularExpression($"/{getLeadF88Request.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadF88>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<LeadF88>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadF88Request.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{getLeadF88Request.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadF88>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<LeadF88>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadF88Request.PosManager))
            {
                var regex = new BsonRegularExpression($"/{getLeadF88Request.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadF88>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                    Builders<LeadF88>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
            }
            return filter;
        }

        public async Task CreateAsync(LeadF88 request)
        {
            try
            {
                await _leadsourceRepository.InsertOneAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ReplaceOneAsync(LeadF88 request)
        {
            try
            {
                await _leadsourceRepository.ReplaceOneAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadF88> GetDetail(string id)
        {
            try
            {
                var result = await _leadsourceRepository.GetCollection().OfType<LeadF88>().FindAsync(x => x.Id == id && !x.IsDeleted);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadF88> GetDetailByIdCard(string idCard)
        {
            try
            {
                LeadF88 leadf88 = await _leadF88Repository.FindOneAsync(x => x.IdCard == idCard);

                return leadf88;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadF88> GetDetailByF88Id(string f88Id)
        {
            try
            {
                LeadF88 lead = await _leadF88Repository.FindOneAsync(x => x.F88Id == f88Id);

                return lead;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LeadF88>> GetByIds(IEnumerable<string> ids)
        {
            try
            {
                var result = await _leadsourceRepository.GetCollection().OfType<LeadF88>().FindAsync(x => ids.Contains(x.Id));
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LeadF88>> GetByIdCardAsync(string idCard)
        {
            return await _leadsourceRepository
                .GetCollection()
                .OfType<LeadF88>()
                .Find(x => x.IdCard == idCard)
                .ToListAsync();
        }
    }
}
