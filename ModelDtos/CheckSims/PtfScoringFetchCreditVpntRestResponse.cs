using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringFetchCreditVpntRestResponse
    {
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("error_desc")]
        public string ErrorDesc { get; set; }
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}
