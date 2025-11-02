using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    [BsonIgnoreExtraElements]
    public class ShinhanReferenceDto
    {
        public string Name { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải là số và có 10 ký tự")]
        public string Phone { get; set; }
        public string Relationship { get; set; }
        public string RelationshipId { get; set; }
    }
}
