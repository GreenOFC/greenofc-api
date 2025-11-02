using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.UserHistories
{
    [BsonIgnoreExtraElements]
    public class UserHistoryResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Action { get; set; }

        public string Endpoint { get; set; }

        public SaleInfomationDto SaleInfomation { get; set; }

        public dynamic Payload { get; set; }
    }
}
