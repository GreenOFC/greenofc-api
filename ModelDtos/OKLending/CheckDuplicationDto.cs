using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.OKLending
{
    public class CheckDuplicationDto
    {
        [JsonProperty("msgDsCd")]
        public string MsgDsCd { get; set; }

        [JsonProperty("agency_code")]
        public string AgencyCode { get; set; }

        [JsonProperty("cust_name")]
        public string CustName { get; set; }

        [JsonProperty("id_no")]
        public string IdNo { get; set; }
    }
}
