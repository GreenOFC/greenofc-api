using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECUpdateStatusResponse
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("body")]
        public ECUpdateStatusDataResponse Body { get; set; }
    }

    public class ECUpdateStatusDataResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
