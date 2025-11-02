using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class CreateLeadEcRequest
    {
        [Required]
        public LeadEcPersonalDto Personal { get; set; }
    }
}
