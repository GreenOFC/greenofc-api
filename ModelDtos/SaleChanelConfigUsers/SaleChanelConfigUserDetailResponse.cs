using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers
{
    [BsonIgnoreExtraElements]
    public class SaleChanelConfigUserDetailResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string UID { get; set; }

        public string ClientID { get; set; }

        public string SecretKey { get; set; }

        public SaleInfomationDto SaleInfo { get; set; }

        public SaleInfomationDto UserModified { get; set; }
    }
}
