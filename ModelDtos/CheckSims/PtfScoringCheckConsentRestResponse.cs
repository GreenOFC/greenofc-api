using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringCheckConsentRestResponse
    {
        [JsonProperty("consent_id")]
        public string ConsentId { get; set; }
        [JsonProperty("consent_type")]
        public string ConsentType { get; set; }
        [JsonProperty("expired_at")]
        public string ExpriredAt { get; set; }
        [JsonProperty("max_usages")]
        public string MaxUsages { get; set; }
        [JsonProperty("used_count")]
        public string UsedCount { get; set; }
    }
}
