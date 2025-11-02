using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Models
{
    public class ProductCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [JsonProperty("productCategoryId")]
        public string ProductCategoryId { get; set; }
        [BsonRequired]
        [JsonProperty("productCategoryName")]
        public string ProductCategoryName { get; set; }
        [JsonProperty("greenType")]
        public string GreenType { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("productLine")]
        public IEnumerable<string> ProductLine { get; set; }
    }
}
