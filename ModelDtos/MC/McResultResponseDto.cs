using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    [BsonIgnoreExtraElements]
    public class McResultResponseDto
    {
        public string Status { get; set; }
        public string ReturnStatus { get; set; }
    }
}
