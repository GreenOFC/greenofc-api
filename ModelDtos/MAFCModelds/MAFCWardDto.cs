using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCWardDto
    {
        [JsonProperty("zipcode")]
        public string ZipCode { get; set; }
        [JsonProperty("zipdesc")]
        public string ZipDesc { get; set; }
        [JsonProperty("city")]
        public string CityId { get; set; }
    }
}
