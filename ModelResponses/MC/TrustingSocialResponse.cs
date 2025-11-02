using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelResponses.MC
{
    [BsonIgnoreExtraElements]
    public class TrustingSocialResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PayLoad { get; set; }
        public string Response { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Debt Debt { get; set; }
        public SaleInfo SaleInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SaleInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Debt
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int AppNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerIdCard { get; set; }
        public string ContractCode { get; set; }
        public string ContractNumber { get; set; }
        public int CurrentDebtPeriod { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public bool IsFollowed { get; set; }
    }
}
