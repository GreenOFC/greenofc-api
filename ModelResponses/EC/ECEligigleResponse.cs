using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECEligigleResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        public ECEligibleDataResponse Data { get; set; }
    }

    public class ECEligibleDataResponse
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
