using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECPresignUrlResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public ECPresignUrlDataResponse Data { get; set; }
    }

    public class ECPresignUrlDataResponse
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("doc_id")]
        public string DocId { get; set; }
    }
}
