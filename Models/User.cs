using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.Users)]
    public class User : BaseEntity
    {
        public User()
        {
            Status = UserStatus.Init;
        }

        [BsonRequired]
        [JsonProperty("userName")]
        public string UserName { get; set; }

        public string MAFCCode { get; set; }

        public string EcDsaCode { get; set; }

        public string EcSaleCode { get; set; }

        public bool IsActiveEc { get; set; }

        [BsonRequired]
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [BsonRequired]
        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [BsonRequired]
        [JsonProperty("phone")]
        public string Phone { get; set; }
        
        [JsonProperty("idCard")]
        public string IdCard { get; set; }
        [JsonProperty("oldIdCard")]
        public string OldIdCard { get; set; }

        [BsonRequired]
        [JsonProperty("userPassword")]
        public string UserPassword { get; set; }

        [BsonRequired]
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("teamLead")]
        public string TeamLead { get; set; }


        [JsonProperty("isActive")]
        public bool IsActive { get; set; }


        public bool IsVerified { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public UserStatus Status { get; set; }

        public string DateOfBirth { get; set; }

        public string PlaceOfBirth { get; set; }

        public string IdCardProvince { get; set; }

        public string IdCardProvinceId { get; set; }

        public string IdCardDate { get; set; }

        public string Gender { get; set; }

        public string OldPhone { get; set; }

        public bool IsTheSameResidentAddress { get; set; }

        public string VerifyCode { get; set; }

        public UserAddress ResidentAddress { get; set; }

        public UserAddress TemporaryAddress { get; set; }

        public UserBankInfo BankInfo { get; set; }

        public UserWorking Working { get; set; }

        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }

        public UserResult Result { get; set; }

        public string Note { get; set; }

        [JsonProperty("roleIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> RoleIds { get; set; }

        public IEnumerable<UserSuspensionHistory> UserSuspensionHistories { get; set; }

        public IEnumerable<GroupDocument> Documents { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ApproveDate { get; set; }

        public SaleChanelInfo SaleChanelInfo { get; set; }

        public ReferrerInfo ReferrerInfo { get; set; }

        public bool? IsManageMultiPos { get; set; }

        public IEnumerable<PosInfo> ManagePosInfos { get; set; }
    }

    public class TeamLead
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }

    public class UserSuspensionHistory
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [BsonRepresentation(BsonType.String)]
        public LeadSourceType LeadSourceType { get; set; }
    }


    public class TeamLeadInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserBankInfo
    {
        public string AccountNo { get; set; }
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserWorking
    {
        public string TaxCode { get; set; }
        public string Title { get; set; }
        public string TitleId { get; set; }
        public UserAddress CompanyAddress { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserAddress
    {
        public string Province { get; set; }
        public string ProvinceId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string Ward { get; set; }
        public string WardId { get; set; }
        public string Street { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SaleInfomation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
    }

    public class UserResult
    {
        public string Reason { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ReferrerInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string IdCard { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class PersonInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string RoleName { get; set; }
    }
}