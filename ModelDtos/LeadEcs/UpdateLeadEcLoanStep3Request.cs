using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcLoanStep3Request
    {
        public string Product { get; set; }
        public string ProductId { get; set; }
        [Range(0, 999999999999)]
        public decimal Amount { get; set; }
        public string Term { get; set; }
        [Range(0, 999)]
        public int? TermId { get; set; }
    }
}
