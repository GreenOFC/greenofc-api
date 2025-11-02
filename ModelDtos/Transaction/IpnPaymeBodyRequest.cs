
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.eWalletTransaction
{
    public class IpnPaymeBodyRequest
    {
        [JsonProperty("x-api-message")]
        public string xApiMessage { get; set; }
    }
}