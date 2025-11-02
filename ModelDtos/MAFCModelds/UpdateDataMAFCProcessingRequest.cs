using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class UpdateDataMAFCProcessingRequest
    {
        [Required]
        public string Status { get; set; }

        [Required]
        public string Step { get; set; }
    }
}
