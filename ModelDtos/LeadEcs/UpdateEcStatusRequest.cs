using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateEcStatusRequest
    {
        [Required]
        public string Status { get; set; }

        public string Reason { get; set; }
    }
}
