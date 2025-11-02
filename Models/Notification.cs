using System;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.Notification)]
    public class Notification : BaseDocument
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string GreenType { get; set; }
        public string RecordId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
