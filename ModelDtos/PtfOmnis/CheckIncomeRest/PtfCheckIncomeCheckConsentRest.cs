using System;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeRest
{
    public class PtfCheckIncomeCheckConsentRestRequest
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public string PhoneNumber { get; set; }
    }
    public class PtfCheckIncomeCheckConsentRestResponse
    {
        [JsonProperty("consent_id")]
        public int ConsentId { get; set; }
        [JsonProperty("consent_type")]
        public string ConsentType { get; set; }
        [JsonProperty("expired_at")]
        public DateTime? ExpiredAt { get; set; }
        [JsonProperty("max_usages")]
        public int MaxUsages { get; set; }
        [JsonProperty("used_count")]
        public int UsedCount { get; set; }
    }

}
