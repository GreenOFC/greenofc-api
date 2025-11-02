using System;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Models.eWalletTransaction
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.TransactionIpn)]
    public class TransactionIpnModel : BaseDocument
    {
        public string Transaction { get; set; }
        public string PartnerTransaction { get; set; }
        public string PaymentId { get; set; }
        public double MerchantId { get; set; }
        public double? StoreId { get; set; }
        public string PayMethod { get; set; }
        public string PayCode { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public double Total { get; set; }
        public string State { get; set; }
        public string Reason { get; set; }
        public string Desc { get; set; }
        public string ExtraData { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string xApiMessage { get; set; }
        public string xApiClient { get; set; }
        public string xApiKey { get; set; }
        public string xApiAction { get; set; }
        public string xApiValidate { get; set; }
        public string Method { get; set; }
    }
}
