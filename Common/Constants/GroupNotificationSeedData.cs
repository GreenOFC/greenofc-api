using _24hplusdotnetcore.Models;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Common.Constants
{
    public class GroupNotificationSeedData
    {

        public static IReadOnlyList<GroupNotification> GroupNotifications = new GroupNotification[]
        {
            new GroupNotification
            {
                GroupName = "Sale",
                GroupCode = "Sale"
            },

            new GroupNotification
            {
                GroupName = "Report",
                GroupCode = "Report"
            }
        };
    }
}
