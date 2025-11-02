using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadHomes
{
    public class UpdateLeadHomeRequest
    {
        [Required]
        public string FullName { get; set; }
        public string IdCard { get; set; }
        [Required]
        public string Phone { get; set; }
        public LeadHomeAddressDto TemporaryAddress { get; set; }
    }
}
