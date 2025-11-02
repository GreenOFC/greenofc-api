using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.Permission)]
    public class Permission: BaseDocument
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }
}
