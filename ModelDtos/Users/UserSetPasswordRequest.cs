using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UserSetPasswordRequest
    {
        [Required]
        [EnumDataType(typeof(VerifyType))]
        public VerifyType Type { get; set; }

        public string UserEmail { get; set; }

        public string Phone { get; set; }

        [Required]
        public string VerifyCode { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
