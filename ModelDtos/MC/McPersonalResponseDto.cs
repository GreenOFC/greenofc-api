using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    [BsonIgnoreExtraElements]
    public class McPersonalResponseDto
    {
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
    }
}
