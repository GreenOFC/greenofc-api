using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.Role
{
    [BsonIgnoreExtraElements]
    public class RoleDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string RoleName { get; set; }
    }
}
