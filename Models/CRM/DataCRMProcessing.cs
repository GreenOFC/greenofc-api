using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models.CRM
{
    public class DataCRMProcessing
    {
        public DataCRMProcessing()
        {
            Status = DataCRMProcessingStatus.InProgress;
            CreateDate = DateTime.UtcNow;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string LeadCrmId { get; set; }
        public string LeadSourceId { get; set; }
        public string Status { get; set; }
        [JsonConverter(typeof(LeadSourceType))]
        [BsonRepresentation(BsonType.String)]
        public LeadSourceType LeadSource { get; set; }
        public string Message { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? FinishDate { get; set; }
    }
}
