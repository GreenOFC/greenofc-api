using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public class CIMBMessage
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }
    }
}
