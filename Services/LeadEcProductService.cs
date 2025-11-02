using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.EC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadEcProductService
    {
        Task SyncAsync();
    }
    public class LeadEcProductService: ILeadEcProductService, IScopedLifetime
    {
        private readonly ILogger<LeadEcProductService> _logger;
        private readonly IMongoRepository<LeadEcProduct> _leadEcProductRepository;
        private readonly IMapper _mapper;
        private readonly ECDataProcessingService _eCDataProcessingService;

        public LeadEcProductService(
            ILogger<LeadEcProductService> logger,
            IMapper mapper,
            IMongoRepository<LeadEcProduct> leadEcProductRepository,
            ECDataProcessingService eCDataProcessingService)
        {
            _logger = logger;
            _leadEcProductRepository = leadEcProductRepository;
            _mapper = mapper;
            _eCDataProcessingService = eCDataProcessingService;
        }

        public async Task SyncAsync()
        {
            try
            {
                var result = await GetAsync();
                await UpdateManyAsync(result);
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

        private async Task<IEnumerable<ECProductListDataResponse>> GetAsync()
        {
            var product = await _eCDataProcessingService.GetProductList();
            return product.Data;
        }

        private async Task UpdateManyAsync(IEnumerable<ECProductListDataResponse> leadEcProducts)
        {
            var leadEcProductInDb = _leadEcProductRepository.FilterBy(x => true);

            var leadEcResourcesInDbMap = leadEcProductInDb.ToDictionary(x => x.EmployeeType, x => x);
            var leadEcResourceMap = leadEcProducts.ToDictionary(x => x.EmployeeType, x => x);

            var leadEcProductToInsert = leadEcProducts
                .Where(x => !leadEcResourcesInDbMap.ContainsKey(x.EmployeeType))
                .Select(x => _mapper.Map<LeadEcProduct>(x))
                .ToList();

            var leadEcProductToDelete = leadEcProductInDb
                .Where(x => !leadEcResourceMap.ContainsKey(x.EmployeeType))
                .ToList();

            var leadEcProductToUpdate = leadEcProductInDb
                .Where(x => leadEcResourceMap.ContainsKey(x.EmployeeType))
                .Select(x =>
                {
                    var leadEcResource = leadEcResourceMap[x.EmployeeType];
                    _mapper.Map(leadEcResource, x);
                    x.ModifiedDate = DateTime.Now;
                    return x;
                })
                .ToList();

            if (leadEcProductToInsert.Any())
            {
                await _leadEcProductRepository.InsertManyAsync(leadEcProductToInsert);
            }
            if (leadEcProductToDelete.Any())
            {
                await _leadEcProductRepository.DeleteManyAsync(leadEcProductToDelete);
            }
            if (leadEcProductToUpdate.Any())
            {
                await _leadEcProductRepository.UpdateManyAsync(leadEcProductToUpdate);
            }
        }
    }
}
