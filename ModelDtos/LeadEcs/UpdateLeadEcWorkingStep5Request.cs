using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcWorkingStep5Request
    {
        [Range(0, 999999999999)]
        public decimal Income { get; set; }
        [Range(0, 999999999999)]
        public decimal? OtherIncome { get; set; }
        public decimal? MonthlyExpenese { get; set; }
    }
}
