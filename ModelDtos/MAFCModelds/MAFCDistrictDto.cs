using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCDistrictDto
    {
        [JsonProperty("lmc_CITYID_C")]
        public string CityId { get; set; }
        [JsonProperty("lmc_CITYNAME_C")]
        public string CityName { get; set; }
        [JsonProperty("lmc_STATE_N")]
        public string StateId { get; set; }
    }
}
