using Refit;
namespace _24hplusdotnetcore.ModelDtos.CheckInitContractModels
{
    public class CheckInitContractRestRequest
    {
        [AliasAs("productId")]
        public int productId { get; set; }
        [AliasAs("customerName")]
        public string customerName { get; set; }
        [AliasAs("citizenId")]
        public string citizenId { get; set; }
        [AliasAs("loanAmount")]
        public decimal loanAmount { get; set; }
        [AliasAs("loanTenor")]
        public decimal loanTenor { get; set; }
        [AliasAs("customerIncome")]
        public decimal customerIncome { get; set; }
        [AliasAs("dateOfBirth")]
        public string dateOfBirth { get; set; }
        [AliasAs("gender")]
        public string gender { get; set; }
        [AliasAs("issuePlace")]
        public string issuePlace { get; set; }
        [AliasAs("issueDateCitizenID")]
        public string issueDateCitizenID { get; set; }
        [AliasAs("hasInsurance")]
        public int hasInsurance { get; set; }
    }

}
