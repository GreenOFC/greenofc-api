using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.UserSuspendeds
{
    public class UpdateUserInUserSuspendedRequest
    {
        [Required]
        public IEnumerable<string> UserNames { get; set; }
    }
}
