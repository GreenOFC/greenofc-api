using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    [BsonIgnoreExtraElements]
    public class MafcPersonalDto
    {
        public string Title { get; set; }
        public string TitleId { get; set; }
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string OldIdCard { get; set; }
        public string IdCardDate { get; set; }
        public string IdCardProvince { get; set; }
        public string IdCardProvinceId { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string SubPhone { get; set; }
        public string DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusId { get; set; }
        public string EducationLevel { get; set; }
        public string EducationLevelId { get; set; }
        public string Email { get; set; }
        public string NoOfDependent { get; set; }
    }
}
