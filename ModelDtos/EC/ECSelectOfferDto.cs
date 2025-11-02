using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECSelectOfferDto
    {
        [JsonProperty("request_id")]
        public string LoanRequestId { get; set; }

        [JsonProperty("partner_code")]
        public string PartnerCode { get; set; }

        [JsonProperty("selected_offer_id")]
        public string SelectedOfferId { get; set; }

        [JsonProperty("selected_offer_amount")]
        public string SelectedOfferAmount { get; set; }

        [JsonProperty("selected_offer_insurance_type")]
        public string SelectedOfferInsuranceType { get; set; }
    }
}
