using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.POS)]
    public class POS : BaseEntity
    {
        public string Name { get; set; }

        public PosManager Manager { get; set; }

        public SaleChanelInfo SaleChanelInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class PosManager
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }
    }

    public class PosInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public PosManager Manager { get; set; }
    }
}
