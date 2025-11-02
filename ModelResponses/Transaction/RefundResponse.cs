using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.Transaction
{
    public class RefundResponse : BaseTransactionResponse
    {
        [JsonProperty("data")]
        public RefundDataResponse Data { get; set; }
    }

    public class RefundDataResponse
    {
        [JsonProperty("partnerTransaction")]
        public string PartnerTransaction { get; set; }

        [JsonProperty("transaction")]
        public string Transaction { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
