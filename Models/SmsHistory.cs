using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.SmsHistory)]
    public class SmsHistory: BaseDocument
    {
        public string PhoneNumber { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string PayLoad { get; set; }

        public string Response { get; set; }
    }
}
