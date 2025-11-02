
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Refit;

namespace _24hplusdotnetcore.Models.MAFC
{
    public class MAFCStatusModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [JsonProperty("id_f1")]
        public string Id_f1 { get; set; }
        [JsonProperty("f1_no")]
        public string F1_no { get; set; }
        [JsonProperty("client_name")]
        public string Client_name { get; set; }
        [JsonProperty("status_f1")]
        public string Status_f1 { get; set; }
        [JsonProperty("f1_time")]
        public string F1_time { get; set; }
        [JsonProperty("rejected_code")]
        public string Rejected_code { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("econtract")]
        public string Econtract { get; set; }
        [JsonProperty("createdTime")]

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
    }
}