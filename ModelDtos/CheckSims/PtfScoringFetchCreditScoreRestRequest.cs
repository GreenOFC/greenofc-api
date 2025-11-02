using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringFetchCreditScoreRestRequest
    {

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("id_type")]
        public string IdType { get; set; }

        [JsonProperty("id_value")]
        public string IdValue { get; set; }
    }
}
