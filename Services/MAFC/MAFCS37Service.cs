using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMAFCS37Service
    {
        Task<MAFCResponse<string>> SubmitAsync(MAFCSubmitS37Request mAFCSubmitS37Request);
        Task<MAFCResponse<JObject>> PollingAsync(MAFCPollingS37Request mAFCPollingS37Request);
    }
    public class MAFCS37Service: IMAFCS37Service, IScopedLifetime
    {
        private readonly ILogger<MAFCS37Service> _logger;
        private readonly IRestMAFCS37Service _restMAFCS37Service;
        private readonly MAFCConfig _mAFCConfig;

        public MAFCS37Service(
            ILogger<MAFCS37Service> logger, 
            IRestMAFCS37Service restMAFCS37Service,
            IOptions<MAFCConfig> mafcConfig)
        {
            _logger = logger;
            _restMAFCS37Service = restMAFCS37Service;
            _mAFCConfig = mafcConfig.Value;
        }

        public async Task<MAFCResponse<string>> SubmitAsync(MAFCSubmitS37Request mAFCSubmitS37Request)
        {
            try
            {
                var request = new MAFCSubmitS37RestRequest
                {
                    VendorCode = _mAFCConfig.S37.VendorCode,
                    IdValue = mAFCSubmitS37Request.IdValue
                };
                var result = await _restMAFCS37Service.SubmitAsync<MAFCResponse<string>>(request);
                return result;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MAFCResponse<JObject>> PollingAsync(MAFCPollingS37Request mAFCPollingS37Request)
        {
            try
            {
                var request = new MAFCPollingS37RestRequest
                {
                    VendorCode = _mAFCConfig.S37.VendorCode,
                    IdValue = mAFCPollingS37Request.IdValue
                };
                var result = await _restMAFCS37Service.PollingAsync<MAFCResponse<JObject>>(request);
                return result;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
