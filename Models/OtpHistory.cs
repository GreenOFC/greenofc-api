using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.OtpHistory)]
    public class OtpHistory: BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReferenceId { get; set; }

        public string ReferenceType { get; set; }

        [JsonConverter(typeof(VerifyType))]
        [BsonRepresentation(BsonType.String)]
        public VerifyType Type { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool IsSendSuccess { get; set; }

        [JsonConverter(typeof(HistoryActionType))]
        [BsonRepresentation(BsonType.String)]
        public HistoryActionType Action { get; set; }

        public string PayLoad { get; set; }

        public string Response { get; set; }
    }
}
