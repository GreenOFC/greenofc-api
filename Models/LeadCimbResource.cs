using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonCollection(MongoCollection.LeadCimbResource)]
    public class LeadCimbResource: BaseDocument
    {
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string Type { get; set; }
        public string Vi { get; set; }
        public string En { get; set; }

        public string Key => $"{Type}{ParentCode}{Code}";
    }

    public enum LeadCimbResourceType
    {
        CITY,
        DISTRICT,
        WARD,
        GENERIC_TERMS_AND_CONDITIONS
    }
}
