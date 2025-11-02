using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringFetchVNPTCreditScoreRestRequest
    {

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        // [JsonProperty("product")]
        // public string Product { get; set; } = "";

        // [JsonProperty("channel")]
        // public string Channel { get; set; } = "";
        [JsonProperty("national_id")]
        public string NationalId { get; set; }
        // [JsonProperty("score_version")]
        // public string ScoreVersion { get; set; } = "";
    }
}