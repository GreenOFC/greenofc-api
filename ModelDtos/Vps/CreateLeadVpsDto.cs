using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Vps
{
    public class CreateLeadVpsDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string IdCard { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
