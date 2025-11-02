using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.News)]
    public class News: BaseEntity
    {
        public string Title { get; set; }
        public string AvatarUrl { get; set; }
        public string Content { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> GroupNotificationIds { get; set; }
        public IEnumerable<string> Tag { get; set; }
    }
}
