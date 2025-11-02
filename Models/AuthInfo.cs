using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.Models
{
    public class AuthInfo
    {
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string RoleId { get; set; }
        public string token { get; set; }
        public string RefreshToken { get; set; }
        public UserStatus Status { get; set; }
    }
}