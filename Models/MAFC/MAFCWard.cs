using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models.MAFC
{
    public class MAFCWard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ZipCode { get; set; }
        public string ZipDesc { get; set; }
        public string CityId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Createdtime { get; set; } = DateTime.Now;
    }
}
