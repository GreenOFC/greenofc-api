using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.SaleChanelConfigUser)]
    public class SaleChanelConfigUser: BaseEntity
    {
        public string UID { get; set; }

        public string ClientID { get; set; }

        public string SecretKey { get; set; }

        public SaleInfomation SaleInfo { get; set; }

        public SaleInfomation UserModified { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SaleChanelConfigUserInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UID { get; set; }

        public string ClientID { get; set; }

        public string SecretKey { get; set; }
    }
}
