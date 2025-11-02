using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfPersonalDto
    {
        public string Title { get; set; }
        public string TitleId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        [MaxLength(20)]
        public string IdCard { get; set; }
        [MaxLength(20)]
        public string OldIdCard { get; set; }
        public string IdCardDate { get; set; }
        public string IdCardExpiredDate { get; set; }
        public string IdCardProvince { get; set; }
        public string IdCardProvinceId { get; set; }
        public string OldIdCardProvince { get; set; }
        public string OldIdCardProvinceId { get; set; }
        public string OldIdCardDate { get; set; }
        public string OldIdCardExpiredDate { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string OldPhone { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusId { get; set; }
        public string NoOfDependent { get; set; }
        public IEnumerable<KeyValueDto> DependentTypes { get; set; }
        public string EducationLevel { get; set; }
        public string EducationLevelId { get; set; }
        public string DependentPerson { get; set; }
        public string DependentPersonId { get; set; }
    }
}
