
using System;
using System.Collections.Generic;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Models.AT
{
    [BsonCollection(MongoCollection.ATTransaction)]
    public class ATTransactionModel: BaseDocument
    {
        [JsonProperty("conversion_id")]
        public string ConversionId { get; set; }
        [JsonProperty("conversion_result_id")]
        public string ConversionResultId { get; set; } = "30";
        [JsonProperty("tracking_id")]
        public string TrackingId { get; set; }
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [JsonProperty("transaction_time")]
        public DateTime TransactionTime { get; set; } = DateTime.Now;
        [JsonProperty("transaction_value")]
        public float TransactionValue { get; set; } = 0;
        [JsonProperty("transaction_discount")]
        public float TransactionDiscount { get; set; } = 0;
        [JsonProperty("extra")]
        public ATExtraModel Extra { get; set; }
        [JsonProperty("is_cpql")]
        public int IsCpql { get; set; } = 0;
        [JsonProperty("items")]
        public IEnumerable<ATItemModel> Items { get; set; }
    }
    public class ATExtraModel
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
    public class ATItemModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "24hplus";
        [JsonProperty("sku")]
        public string Sku { get; set; } = "24hplus";
        [JsonProperty("name")]
        public string Name { get; set; } = "24hplus";
        [JsonProperty("price")]
        public float Price { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; } = 1;
        [JsonProperty("category")]
        public string Category { get; set; } = "24hplus";
        [JsonProperty("category_id")]
        public string CategoryId { get; set; } = "24hplus";
    }
}