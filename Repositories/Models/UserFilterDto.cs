using _24hplusdotnetcore.Common.Enums;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Repositories.Models
{
    public class UserFilterDto
    {
        public string RoleId { get; set; }
        public string PosId { get; set; }
        public string SaleChanelId { get; set; }
        public string TeamLeadUserId { get; set; }
        public bool? IsActive { get; set; }
        public string TextSearch { get; set; }
        public UserStatus? Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<string> Ids { get; set; }
        public IEnumerable<string> UserIds { get; set; }
    }
}
