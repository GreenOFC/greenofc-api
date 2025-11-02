using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using _24hplusdotnetcore.ModelDtos.LeadEcs;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadEcResourceService
    {
        Task SyncAsync();
    }
    public class LeadEcResourceService: ILeadEcResourceService, IScopedLifetime
    {
        private readonly ILogger<LeadEcResourceService> _logger;
        private readonly IMongoRepository<LeadEcResource> _leadEcResourceRepository;
        private readonly IMapper _mapper;

        public LeadEcResourceService(
            ILogger<LeadEcResourceService> logger,
            IMapper mapper,
            IMongoRepository<LeadEcResource> leadEcResourceRepository)
        {
            _logger = logger;
            _leadEcResourceRepository = leadEcResourceRepository;
            _mapper = mapper;
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

        private async Task<IEnumerable<LeadEcResource>> GetAsync()
        {
            var resources = new List<LeadEcResource>();
            using (var reader = new StreamReader(@".\mobileapp\LeadEcs\issue_place_info_UATMR.csv"))
            {
                //skip first row
                await reader.ReadLineAsync();

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');
                    resources.Add(new LeadEcResource { Code = values[0], Type = LeadEcResourceType.ISSUE_PLACE, Vi = values[1] });
                }
            }

            using (var reader = new StreamReader(@".\mobileapp\LeadEcs\address_info_UATMR.csv"))
            {
                //skip first row
                await reader.ReadLineAsync();

                var cities = new List<LeadEcResource>();
                var districts = new List<LeadEcResource>();
                var wards = new List<LeadEcResource>();

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Replace("\"",string.Empty).Split(',');

                    if(!cities.Any(x => x.Type == LeadEcResourceType.CITY && x.Code == values[5] && string.IsNullOrEmpty(x.ParentCode)))
                    {
                        cities.Add(new LeadEcResource { Type = LeadEcResourceType.CITY, Code = values[5], ParentCode = string.Empty, Vi = values[6] });
                    }
                    if(!districts.Any(x => x.Type == LeadEcResourceType.DISTRICT && x.Code == values[3] && x.ParentCode == values[5]))
                    {
                        districts.Add(new LeadEcResource { Type = LeadEcResourceType.DISTRICT, Code = values[3], ParentCode = values[5], Vi = values[4] });
                    }
                    wards.Add(new LeadEcResource { Type = LeadEcResourceType.WARD, Code = values[1], ParentCode = values[3], Vi = values[2] });
                }

                resources.AddRange(cities);
                resources.AddRange(districts);
                resources.AddRange(wards);
            }

            using (var reader = new StreamReader(@".\mobileapp\LeadEcs\mas_bank_info.json"))
            {
                var text = await reader.ReadToEndAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<LeadEcBankInfoMasDto>>>(text);
                var banks = new List<LeadEcResource>();
                var bankBranches = new List<LeadEcResource>();

                foreach (var (key, listOfBank) in dict)
                {
                    foreach (var bank in listOfBank)
                    {
                        if (!banks.Any(x => x.Type == LeadEcResourceType.BANK && x.Code == bank.BankCode && string.IsNullOrEmpty(x.ParentCode)))
                        {
                            banks.Add(new LeadEcResource { Type = LeadEcResourceType.BANK, Code = bank.BankCode, ParentCode = string.Empty, Vi = bank.BankName });
                        }
                        bankBranches.Add(new LeadEcResource { Type = LeadEcResourceType.BANK_BRANCH, Code = bank.BranchCode, ParentCode = bank.BankCode, Vi = bank.BankBranch });
                        
                    }
                }
                resources.AddRange(banks);
                resources.AddRange(bankBranches);
            }
                

            return resources;
        }

        private async Task UpdateManyAsync(IEnumerable<LeadEcResource> leadEcResources)
        {
            var leadEcResourcesInDb = _leadEcResourceRepository.FilterBy(x => true);

            var leadEcResourcesInDbMap = leadEcResourcesInDb.ToDictionary(x => x.Key, x => x);
            var leadEcResourceMap = leadEcResources.ToDictionary(x => x.Key, x => x);

            var leadEcResourcesToInsert = leadEcResources
                .Where(x => !leadEcResourcesInDbMap.ContainsKey(x.Key))
                .ToList();

            var leadEcResourcesToDelete = leadEcResourcesInDb
                .Where(x => !leadEcResourceMap.ContainsKey(x.Key))
                .ToList();

            var leadEcResourcesToUpdate = leadEcResourcesInDb
                .Where(x => leadEcResourceMap.ContainsKey(x.Key))
                .Select(x =>
                {
                    var leadEcResource = leadEcResourceMap[x.Key];
                    _mapper.Map(leadEcResource, x);
                    x.ModifiedDate = DateTime.Now;
                    return x;
                })
                .ToList();

            if (leadEcResourcesToInsert.Any())
            {
                await _leadEcResourceRepository.InsertManyAsync(leadEcResourcesToInsert);
            }
            if (leadEcResourcesToDelete.Any())
            {
                await _leadEcResourceRepository.DeleteManyAsync(leadEcResourcesToDelete);
            }
            if (leadEcResourcesToUpdate.Any())
            {
                await _leadEcResourceRepository.UpdateManyAsync(leadEcResourcesToUpdate);
            }
        }
    }
}
