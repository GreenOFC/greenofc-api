using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.VPS
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.LeadSource)]
    public class LeadVps : LeadSource
    {
        public string  FullName { get; set; }
        public string IdCard { get; set; }
        public string PhoneNumber { get; set; }
    }
}