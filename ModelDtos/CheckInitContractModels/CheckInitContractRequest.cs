using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.CheckInitContractModels
{
    public class CheckInitContractRequest
    {
        [Required]
        public string CustomerId { get; set; }
    }

}
