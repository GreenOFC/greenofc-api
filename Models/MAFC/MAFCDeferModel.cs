
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Models.MAFC
{
    public class MAFCDeferModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [JsonProperty("id_f1")]
        public string Id_f1 { get; set; }
        [JsonProperty("client_name")]
        public string Client_name { get; set; }
        [JsonProperty("defer_code")]
        public string Defer_code { get; set; }
        [JsonProperty("defer_note")]
        public string Defer_note { get; set; }
        [JsonProperty("defer_time")]
        public string Defer_time { get; set; }
        [JsonProperty("createdTime")]

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
    }
}