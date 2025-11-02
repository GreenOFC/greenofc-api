using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    [BsonIgnoreExtraElements]
    public class McLoanResponseDto
    {
        public string Product { get; set; }
    }
}
