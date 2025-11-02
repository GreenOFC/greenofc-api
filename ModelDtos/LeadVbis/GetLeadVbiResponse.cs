using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelDtos.LeadVbis
{
    [BsonIgnoreExtraElements]
    public class GetLeadVbiResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string IdCard { get; set; }

        public string Phone { get; set; }

        public string ExtraPhone { get; set; }

        public LeadVbiAddressDto TemporaryAddress { get; set; }

        [JsonConverter(typeof(LeadSourceStatus))]
        [BsonRepresentation(BsonType.String)]
        public LeadSourceStatus? Status { get; set; }

        public SaleInfoResponse SaleInfo { get; set; }

        public PosInfoDto PosInfo { get; set; }

        public TeamLeadDto TeamLeadInfo { get; set; }
        public SaleChanelDto SaleChanelInfo { get; set; }

        public DateTime CreatedDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
