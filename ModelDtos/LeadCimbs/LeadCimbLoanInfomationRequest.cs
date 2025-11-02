using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class LeadCimbLoanInfomationRequest
    {
        [Required]
        [Range(0, double.MaxValue)]
        public double Amount { get; set; }
    }
}
