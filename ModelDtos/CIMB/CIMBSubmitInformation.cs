using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBSubmitInformation
    {
        [JsonProperty("referenceLoanId")]
        public string ReferenceLoanId { get; set; }

        [JsonProperty("partnerAccountId")]
        public string PartnerAccountId { get; set; }

        [JsonProperty("promotionId")]
        public string PromotionId { get; set; } = "";

        [JsonProperty("partnerCreditScore")]
        public string PartnerCreditScore { get; set; } = "";

        [JsonProperty("saleCode")]
        public string SaleCode { get; set; } = "";

        [JsonProperty("customer")]
        public CIMBCustomerDto Customer { get; set; }

        [JsonProperty("employment")]
        public CIMBEmployment Employment { get; set; }

        [JsonProperty("income")]
        public CIMBIncomeDto Income { get; set; }

        [JsonProperty("loan")]
        public CIMBLoanDto Loan { get; set; }
    }
}
