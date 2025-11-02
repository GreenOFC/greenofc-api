
using System;
using System.Collections.Generic;
using _24hplusdotnetcore.Models.MAFC;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Refit;

namespace _24hplusdotnetcore.Models.MC
{
    public class DataMCPrecheckModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [AliasAs("customerId")]
        public string CustomerId { get; set; }
        [AliasAs("customerName")]
        public string CustomerName { get; set; }
        [AliasAs("idCard")]
        public string IdCard { get; set; }
        [AliasAs("userName")]
        public string UserName { get; set; }
        [AliasAs("saleName")]
        public string SaleName { get; set; }
        [AliasAs("teamLeadInfo")]
        public TeamLead TeamLeadInfo { get; set; }
        [AliasAs("payLoad")]
        public string PayLoad { get; set; }
        [AliasAs("payLoads")]
        public IEnumerable<PayloadModel> Payloads { get; set; }
        [AliasAs("response")]
        public string Response { get; set; }
        [AliasAs("createDate")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [AliasAs("submitedDate")]
        public DateTime SubmitedDate { get; set; }
    }
}