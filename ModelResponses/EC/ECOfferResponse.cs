using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECOfferResponse
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("body")]
        public ECOfferDataResponse Body { get; set; }
    }

    public class ECOfferDataResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }
}