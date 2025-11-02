using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    [BsonIgnoreExtraElements]
    public class UserResultDto
    {
        public string Reason { get; set; }
    }
}
