using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbSaleDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
