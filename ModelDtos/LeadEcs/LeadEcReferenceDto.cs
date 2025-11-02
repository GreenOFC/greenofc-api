using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    [BsonIgnoreExtraElements]
    public class LeadEcReferenceDto
    {
        [StringLength(80, MinimumLength = 1)]
        public string Name { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải là số và có 10 ký tự")]
        public string Phone { get; set; }
        public string Relationship { get; set; }
        public string RelationshipId { get; set; }
    }
}
