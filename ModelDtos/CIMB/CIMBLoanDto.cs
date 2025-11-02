using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBLoanDto
    {
        [JsonProperty("requestAmount")]
        public decimal? RequestAmount { get; set; }

        [JsonProperty("requestTenor")]
        public decimal? RequestTenor { get; set; }

        [JsonProperty("loanPurpose")]
        public string LoanPurpose { get; set; }
    }
}
