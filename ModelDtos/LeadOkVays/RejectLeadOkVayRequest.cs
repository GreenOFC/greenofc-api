using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadOkVays
{
    public class RejectLeadOkVayRequest
    {
        [Required]
        public string Reason { get; set; }
    }
}
