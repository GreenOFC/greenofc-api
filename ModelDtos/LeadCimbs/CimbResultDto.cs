using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbResultDto
    {
        public string ReturnStatus { get; set; }
        public string Reason { get; set; }
        public decimal? ApprovedAmount { get; set; }
    }
}
