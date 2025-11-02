using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UpdateCurrentUserRequest
    {
        public string DateOfBirth { get; set; }

        public string PlaceOfBirth { get; set; }

        [Required]
        public string IdCard { get; set; }
        public string OldIdCard { get; set; }

        public string IdCardProvince { get; set; }

        public string IdCardProvinceId { get; set; }

        public string IdCardDate { get; set; }

        public string Gender { get; set; }

        public string OldPhone { get; set; }
        public bool IsTheSameResidentAddress { get; set; }

        public UserAddressDto ResidentAddress { get; set; }

        public UserAddressDto TemporaryAddress { get; set; }

        public UserBankInfoDto BankInfo { get; set; }

        public UserWorkingDto Working { get; set; }

        public string TeamleadUserId { get; set; }

        public string PosId { get; set; }
        public string RoleName { get; set; }

        public string Note { get; set; }

        public bool IsSubmit { get; set; }
    }

    public class UserBankInfoDto
    {
        public string AccountNo { get; set; }
        public string Name { get; set; }
    }

    public class UserWorkingDto
    {
        public string TaxCode { get; set; }
        public string Title { get; set; }
        public string TitleId { get; set; }
        public UserAddressDto CompanyAddress { get; set; }
    }

    public class UserAddressDto
    {
        public string Province { get; set; }
        public string ProvinceId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string Ward { get; set; }
        public string WardId { get; set; }
        public string Street { get; set; }
    }
}
