using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace _24hplusdotnetcore.ModelDtos.eWalletTransaction
{
    public class CreateTransactionDto
    {

        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("fee")]
        public double Fee { get; set; }
        [JsonProperty("mobileNetworkFee")]
        public double MobileNetworkFee { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("payMethod")]
        public string PayMethod { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        public string SourceData { get; set; }
    }
}
