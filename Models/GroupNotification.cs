using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.GroupNotification)]
    public class GroupNotification : BaseEntity
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
    }
}
