using System.ComponentModel.DataAnnotations;
using Refit;

namespace _24hplusdotnetcore.ModelDtos.AT
{
    public class ATResquestDto
    {
        [AliasAs("name")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [AliasAs("phone")]
        public string Phone { get; set; }
        [AliasAs("dob")]
        public string Dob { get; set; }
        [AliasAs("address")]
        public string Address { get; set; }
        [AliasAs("province")]
        public string Province { get; set; }
        [AliasAs("income")]
        public string Income { get; set; }
        [AliasAs("loanAmount")]
        public string LoanAmount { get; set; }
        [AliasAs("documentType")]
        public string DocumentType { get; set; }
        [AliasAs("incomeSource")]
        public string IncomeSource { get; set; }
        [AliasAs("email")]
        public string Email { get; set; }
        [AliasAs("idCard")]
        public string IdCard { get; set; }
        [AliasAs("providedDoc")]
        public string ProvidedDoc { get; set; }
        [AliasAs("trackingId")]
        public string TrackingId { get; set; }
        [AliasAs("leadSource")]
        public string LeadSource { get; set; }
    }
}
