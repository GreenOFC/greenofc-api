using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos
{
    [BsonIgnoreExtraElements]
    public class KeyValueDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
