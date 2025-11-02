using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBBaseModel
    {
        [JsonProperty("systemCode")]
        public string SystemCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
