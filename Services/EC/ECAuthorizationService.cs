using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public class ECAuthorizationService : IScopedLifetime
    {
        private readonly IECRestAuthorization _ecRestAuthorization;
        private readonly ILogger<ECAuthorizationService> _logger;
        private readonly ECConfig _ecConfig;

        public ECAuthorizationService(IECRestAuthorization ecRestAuthorization, ILogger<ECAuthorizationService> logger, IOptions<ECConfig> ecConfig)
        {
            _ecRestAuthorization = ecRestAuthorization;
            _logger = logger;
            _ecConfig = ecConfig.Value;
        }

        public async Task<string> GetToken()
        {
            try
            {
                // @todo
                // List<KeyValuePair<string, string>> contentKey = new List<KeyValuePair<string, string>>
                //   {
                //       new KeyValuePair<string, string>("grant_type", "client_credentials"),
                //   };

                // FormUrlEncodedContent content = new FormUrlEncodedContent(contentKey);

                // var token = await _ecRestAuthorization.GetToken(content);
                var token = await _ecRestAuthorization.GetToken();
                var tokenDetail = token.ToObject<ECTokenResponse>();
                var bearerToken = string.Format("{0} {1}", "Bearer", tokenDetail.AccessToken);

                return bearerToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
