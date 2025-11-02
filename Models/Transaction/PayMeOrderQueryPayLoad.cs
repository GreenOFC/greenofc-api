using Newtonsoft.Json;

namespace _24hplusdotnetcore.Models.eWalletTransaction
{
    public class PayMeOrderQueryPayLoad
    {
        [JsonProperty("partnerTransaction")]
        public string PartnerTransaction { get; set; }
    }
}
