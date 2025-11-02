using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    [BsonIgnoreExtraElements]
    public class ShinhanResultResponseDto
    {
        public string Status { get; set; }
        public string ReturnStatus { get; set; }
    }
}
