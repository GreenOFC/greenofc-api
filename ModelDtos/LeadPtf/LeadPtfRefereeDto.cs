using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfRefereeDto
    {
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string RelationshipId { get; set; }
        public string Phone { get; set; }
    }
}
