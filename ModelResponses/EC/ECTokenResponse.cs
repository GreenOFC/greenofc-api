using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expire_in")]
        public int ExpireIn { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
