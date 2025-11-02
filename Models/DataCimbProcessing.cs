using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;

namespace _24hplusdotnetcore.Models
{
    [BsonCollection(MongoCollection.DataCimbProcessing)]
    public class DataCimbProcessing: BaseDocument
    {
        public DataCimbProcessing(string customerId)
        {
            CustomerId = customerId;
            Status = DataCimbProcessingStatus.DRAFT;
        }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string PayLoad { get; set; }
        public string Response { get; set; }
    }
}
