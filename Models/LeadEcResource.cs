using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models
{
    [BsonCollection(MongoCollection.LeadEcResource)]
    public class LeadEcResource: BaseDocument
    {
        public string Code { get; set; }
        public string ParentCode { get; set; }
        [JsonConverter(typeof(LeadEcResourceType))]
        [BsonRepresentation(BsonType.String)]
        public LeadEcResourceType Type { get; set; }
        public string Vi { get; set; }

        public string Key => $"{Type}-{ParentCode}-{Code}";
    }

    public enum LeadEcResourceType
    {
        CITY,
        DISTRICT,
        WARD,
        ISSUE_PLACE,
        BANK,
        BANK_BRANCH
    }
}
