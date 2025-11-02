using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.EmailHistory)]
    public class EmailHistory: BaseDocument
    {
        public string Email { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string PayLoad { get; set; }

        public string Response { get; set; }
    }
}
