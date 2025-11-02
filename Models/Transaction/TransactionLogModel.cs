using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using Newtonsoft.Json;
using System;

namespace _24hplusdotnetcore.Models.eWalletTransaction
{
    [BsonCollection(MongoCollection.TransactionLog)]
    public class TransactionLogModel: BaseEntity
    {
        [JsonProperty("transaction")]
        public string Transaction { get; set; }

        [JsonProperty("partnerTransaction")]
        public string PartnerTransaction { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("merchantId")]
        public int MerchantId { get; set; }

        [JsonProperty("storeId")]
        public int StoreId { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("fee")]
        public int Fee { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("extraData")]
        public dynamic ExtraData { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
