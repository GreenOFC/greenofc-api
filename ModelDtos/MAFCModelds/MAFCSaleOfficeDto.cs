using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCSaleOfficeDto
    {
        [JsonProperty("inspectorid")]
        public string InspectorId { get; set; }
        [JsonProperty("inspectorname")]
        public string InspectorName { get; set; }
    }
}
