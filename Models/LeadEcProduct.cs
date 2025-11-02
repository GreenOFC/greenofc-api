using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.LeadEcProduct)]
    public class LeadEcProduct: BaseDocument
    {
        public string EmployeeType { get; set; }
        public string EmployeeDescriptionVi { get; set; }
        public string EmployeeDescriptionEn { get; set; }
        public IEnumerable<LeadEcProductItem> Products { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LeadEcProductItem
    {
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal? LoanMinAmount { get; set; }
        public decimal? LoanMaxAmount { get; set; }
        public decimal? LoanMinTenor { get; set; }
        public decimal? LoanMaxTenor { get; set; }
        public decimal? InterestRate { get; set; }
        public IEnumerable<LeadEcDocument> Documents { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LeadEcDocument
    {
        public string BundleName { get; set; }
        public string BundleCode { get; set; }
        public int? MinRequest { get; set; }
        public IEnumerable<LeadEcDocumentItem> DocumentItems { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LeadEcDocumentItem
    {
        public string DocDescriptionVi { get; set; }
        public string DocDescriptionEn { get; set; }
        public string DocType { get; set; }
        public string DocFormatRequest { get; set; }
    }
}
