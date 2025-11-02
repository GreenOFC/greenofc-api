using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models.MAFC
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.MAFCSaleOffice)]
    public class MAFCSaleOffice: IBaseDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string InspectorId { get; set; }
        public string InspectorName { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Createdtime { get; set; } = DateTime.Now;
    }
}
