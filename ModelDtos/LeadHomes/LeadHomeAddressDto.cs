using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadHomes
{
    [BsonIgnoreExtraElements]
    public class LeadHomeAddressDto
    {
        public DataConfigDto Province { get; set; }
        public DataConfigDto District { get; set; }
        public DataConfigDto Ward { get; set; }
        public string Street { get; set; }
    }
}
