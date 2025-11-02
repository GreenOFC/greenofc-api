using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos
{
    public class CreateManyGroupNotificationUserDto
    {
        public string GroupNotificationId { get; set; }
        public IEnumerable<string> UserNames { get; set; }

    }
}
