using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.UserLogin)]
    public class UserLogin : BaseDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string uuid { get; set; }
        public string ostype { get; set; }
        public string token { get; set; }
        public string registration_token { get; set; }
    }
}