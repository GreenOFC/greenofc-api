using _24hplusdotnetcore.ModelDtos.eWalletTransaction;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.TransactionHistories
{
    [BsonIgnoreExtraElements]
    public class TransactionHistoryResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Action { get; set; }

        public string Endpoint { get; set; }

        public SaleInfomationDto SaleInfomation { get; set; }

        public TransactionResponse Payload { get; set; }
    }

}
