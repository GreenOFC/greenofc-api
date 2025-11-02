using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniGetDataProcessingRequest: PagingRequest
    {
        [Required]
        public string CustomerId { get; set; }
    }
}
