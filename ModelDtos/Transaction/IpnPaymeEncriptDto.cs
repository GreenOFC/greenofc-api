
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.eWalletTransaction
{
    public class IpnPaymeEncriptDto
    {
        public string xApiMessage { get; set; }
        public string xApiClient { get; set; }
        public string xApiKey { get; set; }
        public string xApiAction { get; set; }
        public string xApiValidate { get; set; }
        public string Method { get; set; }
    }
}