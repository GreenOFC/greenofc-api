using System;
using System.Collections.Generic;
using _24hplusdotnetcore.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.MAFC
{
    public class DataMAFCProcessingModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Step { get; set; } = DataProcessingStep.QDE;
        public string Message { get; set; }
        public IEnumerable<PayloadModel> Payloads { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? FinishDate { get; set; }
    }
    public class PayloadModel
    {
        public string Message { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
