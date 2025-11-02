using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class UpdateLeadCimbStep3Request
    {
        [Required]
        public CimbAddressDto ResidentAddress { get; set; }
        [Required]
        public CimbLoanDto Loan { get; set; }
    }
}
