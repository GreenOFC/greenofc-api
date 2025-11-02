using System;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeRest
{
    public class PtfCheckIncomeVerifyOtpRestRequest
    {
        public string Otp { get; set; }
        public string OtpId { get; set; }
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
    }
    public class PtfCheckIncomeVerifyOtpRestResponse
    {
        [JsonProperty("consent_id")]
        public int ConsentId { get; set; }
        [JsonProperty("expired_at")]
        public DateTime? ExpiredAt { get; set; }
        [JsonProperty("max_usages")]
        public int MaxUsages { get; set; }
        [JsonProperty("telco_code")]
        public string TelcoCode { get; set; }
        [JsonProperty("used_count")]
        public int UsedCount { get; set; }
    }

}
