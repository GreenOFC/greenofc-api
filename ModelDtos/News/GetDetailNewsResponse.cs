using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.News
{
    [BsonIgnoreExtraElements]
    public class GetDetailNewsResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string AvatarUrl { get; set; }
        public string Content { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Tag { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> GroupNotificationIds { get; set; }
    }
}
