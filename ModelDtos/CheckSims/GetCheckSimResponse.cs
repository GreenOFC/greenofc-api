using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    [BsonIgnoreExtraElements]
    public class GetCheckSimResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public CheckSimProject Project { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public CheckSimAction Action { get; set; }

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

        public CheckSimTransactionDto Transaction { get; set; }

        public SaleDto SaleInfo { get; set; }

        public PosInfoDto PosInfo { get; set; }

        public SaleChanelDto SaleChanelInfo { get; set; }
        public TeamLeadDto TeamLeadInfo { get; set; }
    }
}
