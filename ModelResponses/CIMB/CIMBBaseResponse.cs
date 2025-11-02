using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public abstract class CIMBBaseResponse
    {
        [JsonProperty("systemCode")]
        public string SystemCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}