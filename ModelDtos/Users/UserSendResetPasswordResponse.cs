using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UserSendResetPasswordResponse
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string UserEmail { get; set; }
    }
}
