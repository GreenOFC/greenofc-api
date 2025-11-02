using _24hplusdotnetcore.ModelDtos.LeadCimbs;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CIMB;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadCimbResourceService
    {
        Task SyncAsync();
    }

    public class LeadCimbResourceService : ILeadCimbResourceService, IScopedLifetime
    {
        private readonly ILogger<LeadCimbResourceService> _logger;
        private readonly IMongoRepository<LeadCimbResource> _leadCimbResourceRepository;
        private readonly IMapper _mapper;
        private readonly ICIMBRestService _cIMBRestService;

        public LeadCimbResourceService(
            ILogger<LeadCimbResourceService> logger,
            IMongoRepository<LeadCimbResource> leadCimbResourceRepository,
            IMapper mapper,
            ICIMBRestService cIMBRestService)
        {
            _logger = logger;
            _leadCimbResourceRepository = leadCimbResourceRepository;
            _mapper = mapper;
            _cIMBRestService = cIMBRestService;
        }

        public async Task SyncAsync()
        {
            try
            {
                var result = await GetAsync();

                var leadCimbResources = result.Data?.Resources?.SelectMany(x =>
                    x.Data?.Select(y =>
                    {
                        var leadCimbResource = _mapper.Map<LeadCimbResource>(y);
                        leadCimbResource.Type = x.Type;
                        return leadCimbResource;
                    }));

                await UpdateManyAsync(leadCimbResources);
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

        private async Task UpdateManyAsync(IEnumerable<LeadCimbResource> leadCimbResources)
        {
            var leadCimbResourcesInDb = _leadCimbResourceRepository.FilterBy(x => true);

            var leadCimbResourcesInDbMap = leadCimbResourcesInDb.ToDictionary(x => x.Key, x => x);
            var leadCimbResourceMap = leadCimbResources.ToDictionary(x => x.Key, x => x);

            var leadCimbResourcesToInsert = leadCimbResources
                .Where(x => !leadCimbResourcesInDbMap.ContainsKey(x.Key))
                .ToList();

            var leadCimbResourcesToDelete = leadCimbResourcesInDb
                .Where(x => !leadCimbResourceMap.ContainsKey(x.Key))
                .ToList();

            var leadCimbResourcesToUpdate = leadCimbResourcesInDb
                .Where(x => leadCimbResourceMap.ContainsKey(x.Key))
                .Select(x =>
                {
                    var leadCimbResource = leadCimbResourceMap[x.Key];
                    _mapper.Map(leadCimbResource, x);
                    x.ModifiedDate = DateTime.Now;
                    return x;
                })
                .ToList();

            if (leadCimbResourcesToInsert.Any())
            {
                await _leadCimbResourceRepository.InsertManyAsync(leadCimbResourcesToInsert);
            }
            if (leadCimbResourcesToDelete.Any())
            {
                await _leadCimbResourceRepository.DeleteManyAsync(leadCimbResourcesToDelete);
            }
            if (leadCimbResourcesToUpdate.Any())
            {
                await _leadCimbResourceRepository.UpdateManyAsync(leadCimbResourcesToUpdate);
            }
        }

        /// <summary>
        /// TODO: Mock data from CIMB resouces
        /// </summary>
        /// <returns></returns>
        private async Task<GetCimbResourceRestResponse> GetAsync()
        {
            var request = new GetCimbResourceRestRequest
            {
                RequestingResources = new List<string> {
                    $"{LeadCimbResourceType.CITY}",
                    $"{LeadCimbResourceType.DISTRICT}",
                    $"{LeadCimbResourceType.WARD}"
                }
            };

            var response = await _cIMBRestService.GetResourceAsync(request);

            return response;
        }
    }
}
