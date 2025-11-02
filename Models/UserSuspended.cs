using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.UserSuspended)]
    public class UserSuspended: BaseEntity
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [BsonRepresentation(BsonType.String)]
        public LeadSourceType LeadSourceType { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public IEnumerable<string> UserIds { get; set; }
    }
}
