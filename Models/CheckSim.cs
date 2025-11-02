using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.CheckSim)]
    public class CheckSim: BaseEntity
    {
        [JsonConverter(typeof(CheckSimProject))]
        [BsonRepresentation(BsonType.String)]
        public CheckSimProject Project { get; set; }

        [JsonConverter(typeof(CheckSimAction))]
        [BsonRepresentation(BsonType.String)]
        public CheckSimAction Action { get; set; }

        public string MobileNetwork { get; set; }
        public string PhoneNumber { get; set; }

        public string IdCard { get; set; }

        public string TypeScore { get; set; }

        public string OTP { get; set; }

        public string AbsolutePath { get; set; }

        public string Payload { get; set; }

        public string Response { get; set; }

        public string Message { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TransactionId { get; set; }

        public CheckSimTransaction Transaction { get; set; }

        public Sale SaleInfo { get; set; }

        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }
        public SaleChanelInfo SaleChanelInfo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CheckSimTransaction
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
