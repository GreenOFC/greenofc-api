using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECUpdateStatusDto
    {
        public string Code { get; set; }

        public string RejectReason { get; set; }

        public string Message { get; set; }

        public ECUpdateStatusDataDto Data { get; set; }
    }

    public class ECUpdateStatusDataDto
    {
        [JsonPropertyName("request_id")]
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonPropertyName("partner_code")]
        [JsonProperty("partner_code")]
        public string PartnerCode { get; set; }

        [JsonPropertyName("proposal_id")]
        [JsonProperty("proposal_id")]
        public string ProposalId { get; set; }

        [JsonPropertyName("contract_number")]
        [JsonProperty("contract_number")]
        public string ContractNumber { get; set; }

        [JsonPropertyName("reject_reason")]
        [JsonProperty("reject_reason")]
        public string RejectReason { get; set; }

        [JsonPropertyName("offer_list")]
        [JsonProperty("offer_list")]
        public IEnumerable<ECOfferListDto> OfferList { get; set; }
    }
}
