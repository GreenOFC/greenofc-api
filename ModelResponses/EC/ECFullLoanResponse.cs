using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECFullLoanResponse
    {
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("body")]
        public ECFullLoanDataResponse Body { get; set; }

    }

    public class ECFullLoanDataResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
