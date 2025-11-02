using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCBankDto
    {
        [JsonProperty("bankid")]
        public string BankId { get; set; }

        [JsonProperty("bankdesc")]
        public string BankDesc { get; set; }
    }
}
