using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.ModelDtos
{
    public class RemoveUserFromGroupNotificationDto
    {
        public string UserId { get; set; }
        public string GroupNotificationId { get; set; }
    }
}
