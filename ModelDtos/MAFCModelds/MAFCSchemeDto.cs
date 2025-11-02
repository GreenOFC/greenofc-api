using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCSchemeDto
    {
        [JsonProperty("schemeid")]
        public string SchemeId { get; set; }
        [JsonProperty("schemename")]
        public string SchemeName { get; set; }
        [JsonProperty("schemegroup")]
        public string SchemeGroup { get; set; }
        public string Product { get; set; }
        public string Maxamtfin { get; set; }
        public string Minamtfin { get; set; }
        public string Maxtenure { get; set; }
        public string Mintenure { get; set; }
    }
}
