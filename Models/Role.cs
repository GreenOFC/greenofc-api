using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.RolesCollection)]
    public class Role: BaseEntity
    {
        [BsonRequired]
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public ICollection<string> PermissionIds { get; set; }
    }
}
