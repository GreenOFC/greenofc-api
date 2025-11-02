using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCAccountCib
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
