using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.OKLending
{
    public class CheckStatusDto
    {
        [JsonProperty("msgDsCd")]
        public string MsgDsCd { get; set; }

        [JsonProperty("agency_code")]
        public string AgencyCode { get; set; }

        [JsonProperty("loanreq_seq")]
        public string LoanreqSeq { get; set; }
    }
}
