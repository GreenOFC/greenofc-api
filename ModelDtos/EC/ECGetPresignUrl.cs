using Newtonsoft.Json;
using Refit;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECGetPresignUrl
    {
        [AliasAs("partner_code")]
        public string PartnerCode { get; set; }

        [AliasAs("request_id")]
        public string RequestId { get; set; }

        [AliasAs("file_name")]
        public string FileName { get; set; }

        [AliasAs("doc_type")]
        public string DocType { get; set; }

        [AliasAs("proposal_id")]
        public string ProposalId { get; set; }
    }
}
