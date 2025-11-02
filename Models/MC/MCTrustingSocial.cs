using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.MC
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.MCTrustingSocial)]
    public class MCTrustingSocial : BaseEntity
    {
        public string PayLoad { get; set; }
        public string Response { get; set; }
        public object PayLoadObj { get; set; }
        public object ResponseObj { get; set; }
    }
}