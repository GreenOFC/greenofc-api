using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface ILeadSourceRepository
    {
        Task<LeadSource> GetDetailAsync(string id);
        Task<LeadSource> CreateLeadSource(LeadSource leadsource);
        Task<LeadSource> DeleteLeadSource(string id);
        Task<IEnumerable<LeadSource>> GetByIds(IEnumerable<string> ids);
        Task ReplaceOneAsync(LeadSource leadsource);
    }

    public class LeadSourceRepository : ILeadSourceRepository, IScopedLifetime
    {
        private readonly ILogger<LeadSourceRepository> _logger;
        private readonly IMongoRepository<LeadSource> _leadsourceRepository;
        public LeadSourceRepository(ILogger<LeadSourceRepository> logger, IMongoRepository<LeadSource> leadsourceRepository)
        {
            _logger = logger;
            _leadsourceRepository = leadsourceRepository;
        }

        public async Task<LeadSource> CreateLeadSource(LeadSource leadsource)
        {
            try
            {
                await _leadsourceRepository.InsertOneAsync(leadsource);
                return leadsource;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadSource> DeleteLeadSource(string id)
        {
            try
            {
                var leadsourceDetail = await _leadsourceRepository.FindByIdAsync(id);
                await _leadsourceRepository.DeleteByIdAsync(leadsourceDetail.Id);
                return leadsourceDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<LeadSource> GetDetailAsync(string id)
        {
            try
            {
                var leadsourceDetail = await _leadsourceRepository.FindByIdAsync(id);
                return leadsourceDetail;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LeadSource>> GetByIds(IEnumerable<string> ids)
        {
            return await _leadsourceRepository.FilterByAsync(x => ids.Contains(x.Id));
        }

        public async Task ReplaceOneAsync(LeadSource leadsource)
        {
            await _leadsourceRepository.ReplaceOneAsync(leadsource);
        }
    }
}
