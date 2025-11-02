using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECSelectOfferResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("data")]
        public ECSelectOfferDataResponse Data { get; set; }
    }

    public class ECSelectOfferDataResponse
    {
        [JsonProperty("proposal_id")]
        public string ProposalId { get; set; }

        [JsonProperty("loan_request_id")]
        public string LoanRequestId { get; set; }
    }
}
