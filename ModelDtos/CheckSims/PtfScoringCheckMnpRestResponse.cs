using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringCheckMnpRestResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("mnp")]
        public int Mnp { get; set; }
        [JsonProperty("telco")]
        public string Telco { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("error_desc")]
        public string ErrorDesc { get; set; }
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}
