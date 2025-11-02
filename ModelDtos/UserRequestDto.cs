
using System.Collections.Generic;
using Refit;

namespace _24hplusdotnetcore.ModelDtos
{
    public class UserRequestDto
    {
        [AliasAs("userId")]
        public string UserId { get; set; }
        [AliasAs("userName")]
        public string UserName { get; set; }
        [AliasAs("fullName")]
        public string FullName { get; set; }
        [AliasAs("userEmail")]
        public string UserEmail { get; set; }
        [AliasAs("phone")]
        public string Phone { get; set; }
        [AliasAs("idCard")]
        public string IdCard { get; set; }
        [AliasAs("roleName")]
        public string RoleName { get; set; }
        [AliasAs("userPassword")]
        public string UserPassword { get; set; }
        [AliasAs("teamLead")]
        public string TeamLead { get; set; }
        [AliasAs("teamLeadInfo")]
        public Models.TeamLead TeamLeadInfo { get; set; }
        [AliasAs("isActive")]
        public bool IsActive { get; set; }
        [AliasAs("roleIds")]
        public IEnumerable<string> RoleIds { get; set; }
    }
    public class UserChangePasswordDto
    {
        [AliasAs("userName")]
        public string UserName { get; set; }
        [AliasAs("newPassword")]
        public string NewPassword { get; set; }
        [AliasAs("oldPassword")]
        public string OldPassword { get; set; }
    }
}
