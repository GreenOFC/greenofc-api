using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.File
{
    [BsonIgnoreExtraElements]
    public class ExportCimbModel
    {
        [Export(ExportName = "modified_time")]
        public DateTime? ModifiedTime { get; set; }

        [Export(ExportName = "sales_name")]
        public string SalesName { get; set; }

        [Export(ExportName = "customer_name")]
        public string CustomerName { get; set; }
    }
}
