using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.Transaction
{
    public class PaymeRefundDto
    {
        [JsonProperty("partnerTransaction")]
        public string PartnerTransaction { get; set; }

        [JsonProperty("transaction")]
        public string Transaction { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
