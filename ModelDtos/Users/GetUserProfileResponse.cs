using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class GetUserProfileResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string EcDsaCode { get; set; }
        public string EcSaleCode { get; set; }
        public string MAFCCode { get; set; }
        public string FullName { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public string OldIdCard { get; set; }
        public string RoleName { get; set; }
        public UserStatus Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> RoleIds { get; set; }
        public TeamLeadInfoDto TeamLeadInfo { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IEnumerable<UserSuspensionHistoryDto> UserSuspensionHistories { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
