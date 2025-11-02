using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCCityDto
    {
        [JsonProperty("stateid")]
        public string StateId { get; set; }
        [JsonProperty("statedesc")]
        public string StateDesc { get; set; }
        [JsonProperty("countryid")]
        public string CountryId { get; set; }
    }
}
