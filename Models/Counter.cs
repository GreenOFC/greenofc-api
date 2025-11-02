using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.Counter)]
    public class Counter: IBaseDocument
    {
        [BsonId]
        public string Id { get; set; }
        public int Seq { get; set; }
    }
}
