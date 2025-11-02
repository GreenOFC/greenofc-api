using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class LeadEcBankInfoMasDto
    {
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        [JsonProperty("bank_branch")]
        public string BankBranch { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }
    }
}
