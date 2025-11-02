using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Roles
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public ICollection<string> PermissionIds { get; set; }
    }
}
