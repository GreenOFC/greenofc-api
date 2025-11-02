using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    [BsonIgnoreExtraElements]
    public class CheckSimTransactionDto
    {
        public string PartnerTransaction { get; set; }
        public double Amount { get; set; }
        public string Desc { get; set; }
        public string PayMethod { get; set; }
        public string Title { get; set; }
        public double Fee { get; set; }
        public double MobileNetworkFee { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public TransactionStatus Status { get; set; } = TransactionStatus.INIT;
    }
}
