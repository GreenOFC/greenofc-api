using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.ImportFile)]
    public class ImportFile : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        public ImportType ImportType { get; set; }
        public string FileName { get; set; }
        public string Extensions { get; set; }
        public long FileSize { get; set; }
        public long TotalRecords { get; set; }
        public bool IsSuccess { get; set; }
        public SaleInfomation SaleInfomation { get; set; }
    }
}
