using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models.PtfOmnis;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.PtfOmnis
{
    public interface IPtfOmniMasterDataService
    {
        Task<IEnumerable<PtfOmniMasterDataResponse>> GetPtfOmniMasterDataAsync(PtfOmniMasterDataRequest request);

        Task SyncAsync();
    }

    public class PtfOmniMasterDataService : IPtfOmniMasterDataService, IScopedLifetime
    {
        private readonly ILogger<PtfOmniMasterDataService> _logger;
        private readonly IMongoRepository<PtfOmniMasterData> _ptfOmniMasterDataRepository;
        private readonly IRestPtfOmniService _restPtfOmniService;
        private readonly IMapper _mapper;
        private readonly PtfOmniConfig _ptfOmniConfig;

        private IReadOnlyList<string> _types = new string[]
        {
            "PTF_LOS_MAS_PRODUCT_TYPE",
            "PTF_LOS_MAS_CREDIT_PRODUCT",
            "PTF_LOS_MAS_INSURANCE_PRODUC",
            "PTF_LOS_MAS_LOAN_CURRENCY",
            "PTF_LOS_MAS_LOAN_PURPOSE",
            "PTF_LOS_MAS_TERM",
            "PTF_LOS_MAS_DUE_DATE",
            "PTF_LOS_MAS_CUSTOMER_TYPE",
            "PTF_LOS_MAS_TITLE",
            "PTF_LOS_MAS_GENDER",
            "PTF_LOS_MAS_ID_TYPE",
            "PTF_LOS_MAS_ISSUE_CITY",
            "PTF_LOS_MAS_SOCIAL_TYPE",
            "PTF_LOS_MAS_CITY",
            "PTF_LOS_MAS_DISTRICT",
            "PTF_LOS_MAS_WARD",
            "PTF_LOS_MAS_MARITAL_STATUS",
            "PTF_LOS_MAS_DEPENDENT_PERSON",
            "PTF_LOS_MAS_EDUCATION",
            "PTF_LOS_MAS_RELATE_PERSON",
            "PTF_LOS_MAS_ECOMOMICAL_STATU",
            "PTF_LOS_MAS_PROFESSION",
            "PTF_LOS_MAS_DISB_METHOD",
            "PTF_LOS_MAS_BANK_NAME",
            "PTF_LOS_MAS_BANK_CITY",
            "PTF_LOS_MAS_BANK_BRANCH",
            "PTF_LOS_MAS_PARTNER_NAME",
            "PTF_LOS_MAS_DEBT_GROUP",
            "PTF_LOS_MAS_SAL_METHOD",
            "PTF_LOS_MAS_CONTRACT_TYPE",
            "PTF_LOS_MAS_CALL_RESULT",
            "PTF_LOS_MAS_INCOME_RESOURCE",
            "PTF_LOS_MAS_EXCEPTION_REASON",
            "PTF_LOS_MAS_DECISION",
            "PTF_LOS_MAS_QUEUE",
            "PTF_LOS_MAS_CAMPAIGN",
            "PTF_LOS_MAS_FV_TYPE",
            "PTF_LOS_MAS_EMPLOYMENT_STT",
            "PTF_LOS_MAS_OCCUPATION",
            "PTF_LOS_MAS_PLACE_OF_BIRTH",
            "PTF_LOS_MAS_QUICK_PROCESS",
            "PTF_LOS_MAS_DISBURS_POLICY",
            "PTF_LOS_MAS_INDUSTRY_T24",
            "PTF_CHANNEL_TYPE",
            "PTF_DOCUMENT_CATEGORY",
            "PTF_DOCUMENT_TYPE"
        };

        public PtfOmniMasterDataService(
            ILogger<PtfOmniMasterDataService> logger,
            IMongoRepository<PtfOmniMasterData> ptfOmniMasterDataRepository,
            IRestPtfOmniService restPtfOmniService,
            IMapper mapper,
            IOptions<PtfOmniConfig> ptfOmniConfig)
        {
            _logger = logger;
            _ptfOmniMasterDataRepository = ptfOmniMasterDataRepository;
            _restPtfOmniService = restPtfOmniService;
            _mapper = mapper;
            _ptfOmniConfig = ptfOmniConfig.Value;
        }

        public async Task<IEnumerable<PtfOmniMasterDataResponse>> GetPtfOmniMasterDataAsync(PtfOmniMasterDataRequest request)
        {
            try
            {
                var records = await _ptfOmniMasterDataRepository.FilterByAsync(x =>
                    (string.IsNullOrEmpty(request.ParentType) || string.Equals(request.ParentType, x.ParentType)) &&
                    (string.IsNullOrEmpty(request.ParentValue) || string.Equals(request.ParentValue, x.ParentValue)) &&
                    request.Types.Contains(x.Type));

                var response = _mapper.Map<IEnumerable<PtfOmniMasterDataResponse>>(records);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SyncAsync()
        {
            try
            {
                _logger.LogInformation("Start sync master data");
                var sw = Stopwatch.StartNew();
                
                var tasks = new List<Task<IEnumerable<PtfOmniMasterDataListResponse>>>();

                foreach (var type in _types)
                {
                    tasks.Add(SyncAsync(type));
                }

                var masterData = await Task.WhenAll(tasks).ConfigureAwait(false);

                _logger.LogInformation($"Master data: {JsonConvert.SerializeObject(masterData)}");

                await UpdateManyAsync(masterData.SelectMany(x => x));

                _logger.LogInformation($"Complete SyncAsync successfully with {sw.ElapsedMilliseconds}ms");
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

        private async Task<IEnumerable<PtfOmniMasterDataListResponse>> SyncAsync(string type)
        {
            try
            {
                _logger.LogInformation($"Start get master data by type: {type}");

                var masterData = await _restPtfOmniService.GetMasterDataListAsync(_ptfOmniConfig.ClientId, _ptfOmniConfig.SecretId, new PtfOmniMasterDataListRequest(type));

                _logger.LogInformation($"Complete get master data by type: {type} successfully: {JsonConvert.SerializeObject(masterData)}");

                return masterData.Data.Items;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new List<PtfOmniMasterDataListResponse>();
        }

        private async Task UpdateManyAsync(IEnumerable<PtfOmniMasterDataListResponse> masterData)
        {
            try
            {
                _logger.LogInformation($"Start instert/update master data to db");

                var masterDataInDb = _ptfOmniMasterDataRepository.FilterBy(x => true).ToList();

                var masterDataInDbMap = masterDataInDb.ToDictionary(x => x.UniqueKey, x => x);
                var masterDataMap = masterData.ToDictionary(x => x.UniqueKey, x => x);

                var masterDataToInsert = masterData
                    .Where(x => !masterDataInDbMap.ContainsKey(x.UniqueKey))
                    .Select(x => _mapper.Map<PtfOmniMasterData>(x))
                    .ToList();

                var masterDataToUpdate = masterDataInDb
                    .Where(x => masterDataMap.ContainsKey(x.UniqueKey))
                    .Select(x =>
                    {
                        var item = masterDataMap[x.UniqueKey];
                        _mapper.Map(item, x);
                        x.ModifiedDate = DateTime.Now;
                        return x;
                    })
                    .ToList();

                if (masterDataToInsert.Any())
                {
                    await _ptfOmniMasterDataRepository.InsertManyAsync(masterDataToInsert);
                    _logger.LogInformation($"Complete instert {masterDataToInsert.Count} records");
                }
                if (masterDataToUpdate.Any())
                {
                    await _ptfOmniMasterDataRepository.UpdateManyAsync(masterDataToUpdate);
                    _logger.LogInformation($"Complete update {masterDataToUpdate.Count} records");
                }

                _logger.LogInformation($"Complete UpdateManyAsync successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
