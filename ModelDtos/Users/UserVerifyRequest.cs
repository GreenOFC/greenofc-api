using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UserVerifyRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Otp { get; set; }
    }
}
