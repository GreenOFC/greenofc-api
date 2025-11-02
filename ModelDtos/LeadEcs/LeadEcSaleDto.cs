using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    [BsonIgnoreExtraElements]
    public class LeadEcSaleDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string EcDsaCode { get; set; }
        public string EcSaleCode { get; set; }
    }
}
