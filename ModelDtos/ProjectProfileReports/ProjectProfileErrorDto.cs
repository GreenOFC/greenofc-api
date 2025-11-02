using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.ProjectProfileReports
{
    [BsonIgnoreExtraElements]
    public class ProjectProfileErrorDto
    {
        public int RowNo { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<string> ErrorColumnNames { get; set; }
    }
}
