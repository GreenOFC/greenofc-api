using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.F88
{
    [BsonIgnoreExtraElements]
    public class F88PersonalDto
    {
        [StringLength(80, MinimumLength = 1)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CMND phải là số và từ 9 đến 12 ký tự")]
        public string IdCard { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải là số và có 10 ký tự")]
        public string Phone { get; set; }
    }
}
