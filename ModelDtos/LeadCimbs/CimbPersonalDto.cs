using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbPersonalDto
    {
        [Required]
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string OldIdCard { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusId { get; set; }
        public string EducationLevel { get; set; }
        public string EducationLevelId { get; set; }
    }
}
