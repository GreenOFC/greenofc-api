using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.LeadPtfCategoryGroup)]
    public class LeadPtfCategoryGroup: BaseDocument
    {
        public string ProductLine { get; set; }
        public IEnumerable<LeadPtfCategory> Categories { get; set; }
    }

    public class LeadPtfCategory
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<LeadPtfProduct> Products { get; set; }
    }

    public class LeadPtfProduct
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
