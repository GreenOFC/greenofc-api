using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.LeadVibs;
using _24hplusdotnetcore.Models;
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
    public interface ILeadVibRepository 
    {
        Task<IEnumerable<GetLeadVibResponse>> GetAsync(IEnumerable<string> userIds, GetLeadVibRequest getLeadVibRequest);
        Task<GetLeadVibResponse> GetRelatedDetailAsync(string id);
        Task<long> CountAsync(IEnumerable<string> userIds, GetLeadVibRequest getLeadVibRequest);

        Task Create(LeadVib leadVib);
        Task<bool> IsExist(string phoneNumber);

        Task<LeadVib> GetDetailAsync(string id);

        Task ReplaceOneAsync(LeadVib leadVib);
    }
    public class LeadVibRepository :  ILeadVibRepository, IScopedLifetime
    {
        private readonly ILogger<LeadVibRepository> _logger;
        private readonly IMongoRepository<LeadSource> _leadsourceRepository;
        private readonly IMongoRepository<LeadVib> _leadVibRepository;

        public LeadVibRepository(
            ILogger<LeadVibRepository> logger,
            IMongoRepository<LeadSource> leadsourceRepository,
            IMongoRepository<LeadVib> leadVibRepository)
        {
            _logger = logger;
            _leadsourceRepository = leadsourceRepository;
            _leadVibRepository = leadVibRepository;
        }

        public async Task<IEnumerable<GetLeadVibResponse>> GetAsync(IEnumerable<string> userIds, GetLeadVibRequest getLeadVibRequest)
        {
            var filter = GetFilter(userIds, getLeadVibRequest);
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Gender", 1 },
                    { "Phone", 1 },
                    { "DateOfBirth", 1 },
                    { "TemporaryAddress", 1 },
                    { "Constitution", 1 },
                    { "Income", 1 },
                    { "Product", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "SaleInfomation", 1},
                    { "PosInfo", 1},
                    { "TeamLeadInfo", 1},
                    { "SaleChanelInfo", 1},
                };
            return await _leadsourceRepository
                .GetCollection()
                .OfType<LeadVib>()
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Skip((getLeadVibRequest.PageIndex - 1) * getLeadVibRequest.PageSize)
                .Limit(getLeadVibRequest.PageSize)
                .Project(projectMapping)
                .As<GetLeadVibResponse>()
                .ToListAsync();
        }

        public async Task<GetLeadVibResponse> GetRelatedDetailAsync(string id)
        {
            var filter = Builders<LeadVib>.Filter.Ne(x => x.IsDeleted, true);
            filter &= Builders<LeadVib>.Filter.Eq(x => x.Id, id);
            var projectMapping = new BsonDocument()
                {
                    { "_id", 1 },
                    { "FullName", 1 },
                    { "IdCard", 1 },
                    { "Gender", 1 },
                    { "Phone", 1 },
                    { "DateOfBirth", 1 },
                    { "TemporaryAddress", 1 },
                    { "Constitution", 1 },
                    { "Income", 1 },
                    { "Product", 1 },
                    { "CreatedDate", 1 },
                    { "ModifiedDate", 1 },
                    { "SaleInfomation", 1},
                };

            return await _leadsourceRepository
                .GetCollection()
                .OfType<LeadVib>()
                .Aggregate()
                .Match(filter)
                .SortByDescending(c => c.ModifiedDate)
                .Project(projectMapping)
                .As<GetLeadVibResponse>()
                .FirstOrDefaultAsync();
        }

        public async Task<long> CountAsync(IEnumerable<string> userIds, GetLeadVibRequest getLeadVibRequest)
        {
            var filter = GetFilter(userIds, getLeadVibRequest);

            var total = await _leadsourceRepository
                .GetCollection()
                .OfType<LeadVib>()
                .Find(filter)
                .CountDocumentsAsync();

            return total;
        }


        private FilterDefinition<LeadVib> GetFilter(IEnumerable<string> userIds, GetLeadVibRequest getLeadVibRequest)
        {
            var filter = Builders<LeadVib>.Filter.Ne(x => x.IsDeleted, true);
            if (!string.IsNullOrEmpty(getLeadVibRequest.TextSearch))
            {
                var regex = new BsonRegularExpression($"/{getLeadVibRequest.TextSearch.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVib>.Filter.Regex(x => x.FullName, regex) |
                    Builders<LeadVib>.Filter.Regex(x => x.IdCard, regex);
            }
            if (userIds?.Any() == true)
            {
                filter &= Builders<LeadVib>.Filter.In(x => x.Creator, userIds);
            }
            if (!string.IsNullOrEmpty(getLeadVibRequest.Sale))
            {
                var regex = new BsonRegularExpression($"/{getLeadVibRequest.Sale.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVib>.Filter.Regex(x => x.SaleInfomation.FullName, regex) |
                    Builders<LeadVib>.Filter.Regex(x => x.SaleInfomation.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadVibRequest.TeamLead))
            {
                var regex = new BsonRegularExpression($"/{getLeadVibRequest.TeamLead.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVib>.Filter.Regex(x => x.TeamLeadInfo.FullName, regex) |
                    Builders<LeadVib>.Filter.Regex(x => x.TeamLeadInfo.UserName, regex);
            }
            if (!string.IsNullOrEmpty(getLeadVibRequest.PosManager))
            {
                var regex = new BsonRegularExpression($"/{getLeadVibRequest.PosManager.ConvertSpecialCharacters()}/i");
                filter &= Builders<LeadVib>.Filter.Regex(x => x.PosInfo.Manager.Name, regex) |
                    Builders<LeadVib>.Filter.Regex(x => x.PosInfo.Manager.UserName, regex);
            }
            return filter;
        }

        public async Task Create(LeadVib leadVib)
        {
            try
            {
                await _leadsourceRepository.InsertOneAsync(leadVib);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> IsExist(string phoneNumber)
        {
            try
            {
                DateTime datefrom = DateTime.Now.AddDays(-15);
           
                var leadVib = (await _leadsourceRepository.GetCollection()
                    .OfType<LeadVib>()
                    .FindAsync(x => x.Phone == phoneNumber && x.ModifiedDate > datefrom && !x.IsDeleted))
                    .FirstOrDefault();

                return leadVib != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadVib> GetDetailAsync(string id)
        {
            try
            {
               return (await _leadsourceRepository.GetCollection().OfType<LeadVib>().FindAsync(x => x.Id == id && !x.IsDeleted)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ReplaceOneAsync(LeadVib leadVib)
        {
            try
            {
                await _leadsourceRepository.ReplaceOneAsync(leadVib);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
