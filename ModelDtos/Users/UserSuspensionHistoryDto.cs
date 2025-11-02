using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    [BsonIgnoreExtraElements]
    public class UserSuspensionHistoryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeadSourceType LeadSourceType { get; set; }
    }
}
