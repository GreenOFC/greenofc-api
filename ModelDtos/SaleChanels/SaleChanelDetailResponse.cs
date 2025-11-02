using _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.SaleChanels
{
    [BsonIgnoreExtraElements]
    public class SaleChanelDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SaleChanelConfigUserId { get; set; }

        public SaleChanelConfigUserInfoDto SaleChanelConfigUserInfo { get; set; }

        public IEnumerable<SaleChanelPosInfoDto> Poses { get; set; }

        public SaleInfomationDto SaleInfo { get; set; }

        public SaleInfomationDto UserModified { get; set; }

        public SaleInfomationDto HeadOfSaleAdmin { get; set; }
    }
}
