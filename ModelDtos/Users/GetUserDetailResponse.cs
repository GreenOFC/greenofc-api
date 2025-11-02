using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    [BsonIgnoreExtraElements]
    public class GetUserDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string MAFCCode { get; set; }
        public string EcDsaCode { get; set; }
        public string EcSaleCode { get; set; }
        public string FullName { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public string OldIdCard { get; set; }
        public string RoleName { get; set; }
        public bool? IsActive { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string IdCardProvince { get; set; }
        public string IdCardProvinceId { get; set; }
        public string IdCardDate { get; set; }
        public UserStatus Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> RoleIds { get; set; }
        public IEnumerable<UserSuspensionHistoryDto> UserSuspensionHistories { get; set; }
        public IEnumerable<GroupDocumentDto> Documents { get; set; }
        public string Gender { get; set; }

        public string OldPhone { get; set; }

        public UserAddressDto ResidentAddress { get; set; }

        public UserAddressDto TemporaryAddress { get; set; }

        public UserBankInfoDto BankInfo { get; set; }

        public UserWorkingDto Working { get; set; }

        public TeamLeadDto TeamLeadInfo { get; set; }
        public TeamLeadDto AsmInfo { get; set; }

        public PosInfoDto PosInfo { get; set; }

        public UserResultDto Result { get; set; }

        public string Note { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsTheSameResidentAddress { get; set; }

        public SaleChanelDto SaleChanelInfo { get; set; }

        public ReferrerInfoDto ReferrerInfo { get; set; }

        public bool? IsManageMultiPos { get; set; }

        public IEnumerable<PosInfoDto> ManagePosInfos { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
