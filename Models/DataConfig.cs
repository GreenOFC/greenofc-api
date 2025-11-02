using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.DataConfig)]
    public class DataConfig: BaseDocument
    {
        public string GreenType { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
