using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories.CRM
{
    public interface ILeadCrmRepository
    {
        Task CreateAsyn(LeadCrm request);
        void ReplaceOne(LeadCrm request);
        Task ReplaceOneAsync(LeadCrm request);
        Task<LeadCrm> GetDetailByPotentionNo(string potentialNo);
        Task<LeadCrm> GetDetailByPhoneNumber(string phoneNumber);
        IEnumerable<LeadCrm> GetByIds(IEnumerable<string> ids);
        Task<IEnumerable<LeadCrm>> GetByLeadCrmIdsAsync(IEnumerable<string> leadCrmIds);
    }

    public class LeadCrmRepository : ILeadCrmRepository, ISingletonService
    {
        private readonly ILogger<LeadCrmRepository> _logger;

        private readonly IMongoRepository<LeadSource> _leadsourceRepository;
        private readonly IMongoRepository<LeadCrm> _leadCrmRepository;

        public LeadCrmRepository(
            ILogger<LeadCrmRepository> logger,
            IMongoRepository<LeadSource> leadsourceRepository,
            IMongoRepository<LeadCrm> leadCrmRepository
            )
        {
            _logger = logger;
            _leadsourceRepository = leadsourceRepository;
            _leadCrmRepository = leadCrmRepository;
        }

        public async Task CreateAsyn(LeadCrm leadCrm)
        {
            try
            {
                leadCrm.Createdtime ??= DateTime.Now;

                await _leadsourceRepository.InsertOneAsync(leadCrm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<LeadCrm> GetByIds(IEnumerable<string> ids)
        {
            try
            {
                return _leadsourceRepository.GetCollection().OfType<LeadCrm>().Find(c => ids.Contains(c.Id)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LeadCrm>> GetByLeadCrmIdsAsync(IEnumerable<string> leadCrmIds)
        {
            try
            {
                var result = await _leadsourceRepository.GetCollection().OfType<LeadCrm>().FindAsync(c => leadCrmIds.Contains(c.LeadCrmId));

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadCrm> GetDetailByPhoneNumber(string phoneNumber)
        {
            try
            {
                var datefrom = DateTime.Now.AddDays(-15);
                var filter = Builders<LeadCrm>.Filter.Gte(c => c.Createdtime, datefrom);
                filter &= Builders<LeadCrm>.Filter.Eq(c => c.Cf854, phoneNumber);

                var result = await _leadsourceRepository.GetCollection().OfType<LeadCrm>().Find(filter).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadCrm> GetDetailByPotentionNo(string potentialNo)
        {
            try
            {
                var result = await _leadsourceRepository.GetCollection().OfType<LeadCrm>().FindAsync(c => c.PotentialNo == potentialNo);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void ReplaceOne(LeadCrm request)
        {
            try
            {
                request.Modifiedtime = DateTime.Now;
                _leadsourceRepository.ReplaceOne(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ReplaceOneAsync(LeadCrm leadCrm)
        {
            try
            {
                leadCrm.Modifiedtime = DateTime.Now;

                await _leadsourceRepository.ReplaceOneAsync(leadCrm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
