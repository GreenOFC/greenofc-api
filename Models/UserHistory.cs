using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.UserHistory)]
    public class UserHistory : BaseEntity
    {
        public UserHistory(SaleInfomation saleInfomation, User payload, string action, string endpoint)
        {
            SaleInfomation = saleInfomation;
            Payload = payload;
            Creator = saleInfomation?.Id;
            Action = action;
            Endpoint = endpoint;
        }

        public string Action { get; set; }

        public string Endpoint { get; set; }

        public SaleInfomation SaleInfomation { get; set; }

        public User Payload { get; set; }
    }
}
