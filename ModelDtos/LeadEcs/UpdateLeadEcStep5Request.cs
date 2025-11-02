using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcStep5Request
    {
        [Required]
        public UpdateLeadEcWorkingStep5Request Working { get; set; }
        [Required]
        public LeadEcDisbursementInformationDto DisbursementInformation { get; set; }
    }
}
