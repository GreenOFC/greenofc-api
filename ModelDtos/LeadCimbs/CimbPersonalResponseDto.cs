using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbPersonalResponseDto
    {
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
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
