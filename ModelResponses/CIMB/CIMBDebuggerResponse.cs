using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public class CIMBDebuggerResponse
    {
        [JsonProperty("message")]
        public CIMBMessage Message { get; set; }

        [JsonProperty("response")]
        public CIMBResponse Response { get; set; }
        public string Status { get; set; }
    }
}
