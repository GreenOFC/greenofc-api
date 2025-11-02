using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    [BsonIgnoreExtraElements]
    public class ShinhanPersonalResponseDto
    {
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
    }
}
