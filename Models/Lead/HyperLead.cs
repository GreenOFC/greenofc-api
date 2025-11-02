using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.Lead
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.HyperLead)]
    public class HyperLead : BaseEntity
    {
        public string ConversionId { get; set; }
        public string ClickId { get; set; }
        public string ConversionSaleAmount { get; set; }
        public string ConversionTime { get; set; }
        public string ProductUrl { get; set; }
        public string ProductCategory { get; set; }
        public string OfferId { get; set; }
        public string ConversionStatusCode { get; set; }
        public string ConversionPublisherPayout { get; set; }
        public string ConversionModifiedTime { get; set; }
        public string ProductSku { get; set; }
        public string TransactionId { get; set; }
        public string ConversionStatus { get; set; }
        public string ClickTime { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryId { get; set; }
        public string StatusMessage { get; set; }
        public string AffSub1 { get; set; }
        public string AffSub2 { get; set; }
        public string AffSub3 { get; set; }
        public string AffSub4 { get; set; }
        
        public SaleInfomation SaleInfomation { get; set; }
        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }

        public SaleChanelInfo SaleChanelInfo { get; set; }
    }
}