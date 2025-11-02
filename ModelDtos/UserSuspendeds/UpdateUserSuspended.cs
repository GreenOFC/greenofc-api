using _24hplusdotnetcore.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.UserSuspendeds
{
    public class UpdateUserSuspended
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public LeadSourceType LeadSourceType { get; set; }
    }
}
