
using System;
using System.ComponentModel.DataAnnotations;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Models;

namespace _24hplusdotnetcore.Models.F88
{
    [BsonCollection(MongoCollection.F88Notification)]
    public class F88Notification: BaseEntity
    {
        public string TransactionId { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string DetailStatus { get; set; }
        public string LoanAmount { get; set; }
    }
}
