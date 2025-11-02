using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Globalization;
using System.Linq;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.ProjectProfileReportDetail)]
    public class ProjectProfileReportDetail : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProjectProfileReportId { get; set; }

        public string Project { get; set; }

        public string AppCreationDate { get; set; }
        
        public string AppCreationMonth { get; set; }

        public string LastStatusChangeDate { get; set; }

        public string DisbursementDate { get; set; }

        public string ContractNumber { get; set; }

        public string ProfileCode { get; set; }

        public string CustomerName { get; set; }

        public string ProductName { get; set; }

        public string BHKV { get; set; }

        public decimal? SuggestedAmount { get; set; }

        public decimal? DisbursementAmount { get; set; }

        public string ProfileStatus { get; set; }

        public string DEErrorReturned { get; set; }

        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }

        public SaleInfomation SaleInfomation { get; set; }

        public SaleChanelInfo SaleChanelInfo { get; set; }

        public string Unit { get; set; }
    }
}
