using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.Config
{
    public class PaymeConfigResponse
    {
        public string AppToken { get; set; }
        public string SecretKey { get; set; }
        public string AppId { get; set; }
        public string SdkEnv { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
