using Newtonsoft.Json;

namespace _24hplusdotnetcore.Models.eWalletTransaction
{
    public class PayMePayload
    {
        [JsonProperty("partnerTransaction")]
        public string PartnerTransaction { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("payMethod")]
        public string PayMethod { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ipnUrl")]
        public string IpnUrl { get; set; }

        [JsonProperty("extraData")]
        public string ExtraData { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("payData")]
        public dynamic PayData { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("billingInfo")]
        public dynamic BillingInfo { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty("failedUrl")]
        public string FailedUrl { get; set; }

        [JsonProperty("redirectTime")]
        public string RedirectTime { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }
    }
}
