using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.CheckCustomer)]
    public class CheckCustomer : BaseEntity
    {
        public string FileName { get; set; }

        public int TotalIdCards { get; set; }

        public SaleInfomation SaleInfomation { get; set; }
    }
}
