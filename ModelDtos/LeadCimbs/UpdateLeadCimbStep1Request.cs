using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class UpdateLeadCimbStep1Request
    {
        [Required]
        public CimbPersonalDto Personal { get; set; }
    }
}
