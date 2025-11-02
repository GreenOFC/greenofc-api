using System;
using System.Collections.Generic;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.eWalletTransaction
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.Transaction)]
    public class TransactionModel : BaseEntity
    {
        public string PartnerTransaction { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public double MobileNetworkFee { get; set; }
        public string Desc { get; set; }
        public string PayMethod { get; set; }
        public string Title { get; set; }
        public string SourceData { get; set; }
        public string IpnUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string FailedUrl { get; set; }

        [BsonRepresentation(BsonType.String)]
        public TransactionStatus Status { get; set; } = TransactionStatus.INIT;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? SuccessDate { get; set; }
        public PayMeOrderData PaymeResponse { get; set; }

        public string Type { get; set; }

        [BsonRepresentation(BsonType.String)]
        public BillType BillType { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string BillId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public BillStatus BillStatus { get; set; } = BillStatus.INIT;
        public string BillPhoneNumber { get; set; }
        public string BillIdCard { get; set; }
        public IEnumerable<BillStepHistories> BillStepHistories { get; set; }
        
        public string Payload { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public Sale SaleInfo { get; set; }
        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }

        public SaleChanelInfo SaleChanelInfo { get; set; }
    }
    public class PayMeOrderData
    {
        public string URL { get; set; }
        public string Transaction { get; set; }
    }
    public class BillStepHistories
    {
        [BsonRepresentation(BsonType.String)]
        public BillStatus Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
