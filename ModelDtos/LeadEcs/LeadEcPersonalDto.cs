using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    [BsonIgnoreExtraElements]
    public class LeadEcPersonalDto : IValidatableObject
    {
        [StringLength(80, MinimumLength = 1)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }
        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CMND phải là số và từ 9 đến 12 ký tự")]
        public string IdCard { get; set; }
        public string IdCardProvince { get; set; }
        public string IdCardProvinceId { get; set; }
        public string IdCardDate { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải là số và có 10 ký tự")]
        public string Phone { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(DateOfBirth))
            {
                string[] format = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };

                if (!DateTime.TryParseExact(DateOfBirth, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth))
                {
                    yield return new ValidationResult("Ngày sinh của khách hàng không đúng định dạng", new string[] { nameof(DateOfBirth) });
                }
                else
                {
                    if (dateOfBirth > DateTime.Now.AddYears(-20) || dateOfBirth < DateTime.Now.AddYears(-60))
                    {
                        yield return new ValidationResult("Ngày/tháng/năm sinh từ 20 tuổi đến 60 tuổi", new string[] { nameof(DateOfBirth) });
                    }

                    else if (!string.IsNullOrEmpty(IdCardDate))
                    {
                        if (!DateTime.TryParseExact(IdCardDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime idCardDate))
                        {
                            yield return new ValidationResult("Ngày cấp CMND không đúng định dạng", new string[] { nameof(IdCardDate) });
                        }
                        else if (idCardDate > DateTime.Now || idCardDate < DateTime.Now.AddYears(-15))
                        {
                            yield return new ValidationResult("Ngày cấp thẻ căn cước phải trong vòng 15 năm", new string[] { nameof(IdCardDate) });
                        }
                    }
                }
            }
        }
    }
}
