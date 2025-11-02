using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Vps
{
    public class UpdateLeadVpsDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string IdCard { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
