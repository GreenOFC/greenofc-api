using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.ModelDtos.ProjectProfileReports
{
    [BsonIgnoreExtraElements]
    public class ProjectProfileReportResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string FileName { get; set; }

        public int TotalRecords { get; set; }

        public int TotalSuccessfulRecords { get; set; }

        public int TotalFailedRecords { get; set; }

        public SaleInfomationDto SaleInfomation { get; set; }
    }
}
