using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfCheckSimRequest
    {
        [Required]
        public string PhoneNumber { get; set; }

        public string IdCard { get; set; }
    }
}
