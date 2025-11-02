using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfShortAddressDto
    {
        public string Province { get; set; }
        public string ProvinceId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string Ward { get; set; }
        public string WardId { get; set; }
        public string Street { get; set; }
    }
}
