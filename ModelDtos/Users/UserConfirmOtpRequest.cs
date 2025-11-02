using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UserConfirmOtpRequest
    {
        public string UserRequest { get; set; }

        [Required]
        public string Otp { get; set; }
    }
}
