using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelResponses
{
    [BsonIgnoreExtraElements]
    public class GroupNotificationUserResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public UserLoginLookupResult UserLoginLookupResult { get; set; }
        public GroupNotificationLookupResult GroupNotificationLookupResult { get; set; }
        public UserLookupResult UserLookupResult { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserLoginLookupResult
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string registration_token { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class GroupNotificationLookupResult
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserLookupResult
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string UserEmail { get; set; }
    }
}
