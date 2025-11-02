using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcStep1Request
    {
        [Required]
        public LeadEcPersonalDto Personal { get; set; }
    }
}
