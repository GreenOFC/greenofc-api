using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;

namespace _24hplusdotnetcore.Models.F88
{
    [BsonCollection(MongoCollection.DataF88Processing)]
    public class DataF88Processing : BaseDocument
    {
        public string LeadF88Id { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string PayLoad { get; set; }
        public string Response { get; set; }
    }
}
