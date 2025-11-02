using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers
{
    public class SaleChanelConfigUserCreateRequest
    {
        [Required]
        public string UID { get; set; }

        [Required]
        public string ClientID { get; set; }

        [Required]
        public string SecretKey { get; set; }
    }
}
