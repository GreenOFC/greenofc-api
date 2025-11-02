using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniCancelApplicationRequest
    {
        [Required]
        public string Reason { get; set; }
    }
}
