using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Models.eWalletTransaction;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.Transaction
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.TransactionHistory)]
    public class TransactionHistory : BaseEntity
    {
        public TransactionHistory(SaleInfomation saleInfomation, TransactionModel transactionModel, string action, string endpoint)
        {
            SaleInfomation = saleInfomation;
            Payload = transactionModel;
            Creator = saleInfomation.Id;
            Action = action;
            Endpoint = endpoint;
        }

        public string Action { get; set; }

        public string Endpoint { get; set; }

        public SaleInfomation SaleInfomation { get; set; }

        public TransactionModel Payload { get; set; }
    }
}
