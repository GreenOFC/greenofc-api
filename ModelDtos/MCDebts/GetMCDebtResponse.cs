using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MCDebts
{
    [BsonIgnoreExtraElements]
    public class GetMCDebtResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AppNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerIdCard { get; set; }
        public string ContractCode { get; set; }
        public string ContractNumber { get; set; }
        public int CurrentDebtPeriod { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public bool IsFollowed { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<SaleInfoResponse> SaleInfo { get; set; }
    }
}
