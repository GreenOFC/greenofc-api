using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.ProjectProfileReport)]
    public class ProjectProfileReport : BaseEntity
    {
        public string FileName { get; set; }

        public int TotalRecords { get; set; }

        public int TotalSuccessfulRecords { get; set; }

        public int TotalFailedRecords { get; set; }

        public SaleInfomation SaleInfomation { get; set; }
    }

}
