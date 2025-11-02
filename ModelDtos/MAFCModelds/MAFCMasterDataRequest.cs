using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCMasterDataRequest
    {
        [JsonProperty("msgName")]
        public string MsgName { get; set; }
    }
}
