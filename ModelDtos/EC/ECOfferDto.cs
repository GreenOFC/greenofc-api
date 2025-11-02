using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECOfferDto
    {
        [JsonPropertyName("code")]
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        [JsonProperty("data")]
        public ECOfferDataDto Data { get; set; }
    }

    public class ECOfferDataDto
    {
        [JsonPropertyName("request_id")]
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonPropertyName("proposal_id")]
        [JsonProperty("proposal_id")]
        public string ProposalId { get; set; }

        [JsonPropertyName("offer_list")]
        [JsonProperty("offer_list")]
        public IEnumerable<ECOfferListDto> OfferList { get; set; }
    }

    public class ECOfferListDto
    {
        [JsonPropertyName("offer_id")]
        [JsonProperty("offer_id")]
        public decimal? OfferId { get; set; }

        [JsonPropertyName("offer_amount")]
        [JsonProperty("offer_amount")]
        public decimal? OfferAmount { get; set; }

        [JsonPropertyName("interest_rate")]
        [JsonProperty("interest_rate")]
        public float? InterestRate { get; set; }

        [JsonPropertyName("monthly_installment")]
        [JsonProperty("monthly_installment")]
        public decimal? MonthlyInstallment { get; set; }

        [JsonPropertyName("offer_tenor")]
        [JsonProperty("offer_tenor")]
        public decimal? OfferTenor { get; set; }

        [JsonPropertyName("min_financed_amount")]
        [JsonProperty("min_financed_amount")]
        public decimal? MinFinancedAmount { get; set; }

        [JsonPropertyName("max_financed_amount")]
        [JsonProperty("max_financed_amount")]
        public decimal? MaxFinancedAmount { get; set; }

        [JsonPropertyName("offer_variant")]
        [JsonProperty("offer_variant")]
        public decimal? OfferVariant { get; set; }

        [JsonPropertyName("offer_var")]
        [JsonProperty("offer_var")]
        public decimal? OfferVar { get; set; }

        [JsonPropertyName("offer_type")]
        [JsonProperty("offer_type")]
        public string OfferType { get; set; }

        [JsonPropertyName("insurance_list")]
        [JsonProperty("insurance_list")]
        public IEnumerable<ECOfferInsuranceListDto> InsuranceList { get; set; }
    }

    public class ECOfferInsuranceListDto
    {
        [JsonPropertyName("insurance_type")]
        [JsonProperty("insurance_type")]
        public string InsuranceType { get; set; }

        [JsonPropertyName("insurance_amount")]
        [JsonProperty("insurance_amount")]
        public decimal? InsuranceAmount { get; set; }

        [JsonPropertyName("percentage_insurance")]
        [JsonProperty("percentage_insurance")]
        public float? PercentageInsurance { get; set; }

        [JsonPropertyName("base_calculation")]
        [JsonProperty("base_calculation")]
        public string BaseCalculation { get; set; }
    }
}
