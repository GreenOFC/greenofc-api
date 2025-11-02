using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos
{
    [BsonIgnoreExtraElements]
    public class GroupNotificationUserDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string GroupNotificationId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
