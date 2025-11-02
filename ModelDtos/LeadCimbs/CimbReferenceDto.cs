using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbReferenceDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }
        public string RelationshipId { get; set; }
    }
}
