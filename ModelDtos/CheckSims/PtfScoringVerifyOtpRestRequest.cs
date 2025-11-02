using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringVerifyOtpRestRequest
    {
        [JsonProperty("otp")]
        public string Otp { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }
}
