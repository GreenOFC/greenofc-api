using System.Collections.Generic;
using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.Models
{
    public class ResponseLoginInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string token { get; set; }
        public string Phone { get; set; }
        public string UserEmail { get; set; }
        public string registration_token { get; set; }
        public long unReadNoti { get; set; }
        public string mafcCode { get; set; }
        public string ecDsaCode { get; set; }
        public UserStatus Status { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}