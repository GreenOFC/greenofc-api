using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models.MC
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.MCYearVerifyHistory)]
    public class MCYearVerifyHistory : BaseEntity
    {
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        [JsonConverter(typeof(HistoryActionType))]
        [BsonRepresentation(BsonType.String)]
        public HistoryActionType Action { get; set; }

        public string PayLoad { get; set; }
        public string Response { get; set; }
    }
}
