using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECProductListDto
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        [JsonProperty("partner_code")]
        public string PartnerCode { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
