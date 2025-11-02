using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models.MC
{
    public class MCKios
    {
        public string Id { get; set; }
        public string KioskCode { get; set; }
        public string KioskAddress { get; set; }
        public string KioskProvince { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedDateTime { get; set; } = DateTime.Now;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdatedDateTime { get; set; }
    }
}
