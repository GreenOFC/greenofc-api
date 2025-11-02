using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models.MAFC
{
    public class MAFCDistrict
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Createdtime { get; set; } = DateTime.Now;
    }
}
