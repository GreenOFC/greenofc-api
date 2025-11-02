using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.CIMB
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.CIMBOnBoardingCheckingProcessing)]
    public class CIMBOnBoardingCheckingProcessing : BaseDocument
    {
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Payload { get; set; }
    }
}
