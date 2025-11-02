using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class CreateLeadCimbRequest
    {
        [Required]
        public CimbPersonalDto Personal { get; set; }
        public string MobileVersion { get; set; }
    }
}
