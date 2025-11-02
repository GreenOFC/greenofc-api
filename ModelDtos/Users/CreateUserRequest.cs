using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string FullName { get; set; }
        public string MAFCCode { get; set; }
        public string EcDsaCode { get; set; }
        public string EcSaleCode { get; set; }
        public string UserEmail { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string IdCard { get; set; }
        public string OldIdCard { get; set; }
        public string RoleName { get; set; }

        public string TeamLeadUserId { get; set; }
        public IEnumerable<string> RoleIds { get; set; }
        public string PosId { get; set; }

        public UserBankInfoDto BankInfo { get; set; }

        public string DateOfBirth { get; set; }

        public IEnumerable<GroupDocumentDto> Documents { get; set; }

        public string Gender { get; set; }

        public string IdCardDate { get; set; }

        public string IdCardProvince { get; set; }

        public string IdCardProvinceId { get; set; }

        public string Note { get; set; }

        public UserAddressDto ResidentAddress { get; set; }

        public UserAddressDto TemporaryAddress { get; set; }

        public string OldPhone { get; set; }

        public string PlaceOfBirth { get; set; }

        public UserWorkingDto Working { get; set; }

        public bool? IsManageMultiPos { get; set; }

        public IEnumerable<string> ManagePosIds { get; set; }
    }
}
