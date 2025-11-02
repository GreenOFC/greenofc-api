using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public class CIMBBadResponse : CIMBBaseResponse
    {
        [JsonProperty("data")]
        public CIMBBadDataResponse Data { get; set; }

        [JsonProperty("debugError")]
        public CIMBDebuggerResponse DebugError { get; set; }
    }

    public class CIMBBadDataResponse
    {
        public CIMBMessage Message { get; set; }
        public CIMBResponse Response { get; set; }
    }
}
