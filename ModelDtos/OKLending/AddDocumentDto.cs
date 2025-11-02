using Newtonsoft.Json;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.OKLending
{
    public class Array
    {
        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }
    }

    public class AddDocumentDto
    {
        [JsonProperty("msgDsCd")]
        public string MsgDsCd { get; set; }

        [JsonProperty("agency_code")]
        public string AgencyCode { get; set; }

        [JsonProperty("loanreq_seq")]
        public string LoanreqSeq { get; set; }

        [JsonProperty("array")]
        public List<Array> Array { get; set; }
    }
}
