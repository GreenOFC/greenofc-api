using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models.MAFC
{
    public class MAFCScheme
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SchemeId { get; set; }
        public string SchemeName { get; set; }
        public string SchemeGroup { get; set; }
        public string Product { get; set; }
        public string Maxamtfin { get; set; }
        public string Minamtfin { get; set; }
        public string Maxtenure { get; set; }
        public string Mintenure { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Createdtime { get; set; } = DateTime.Now;
    }
}
