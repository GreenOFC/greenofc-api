using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
using _24hplusdotnetcore.ModelDtos.Users;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.eWalletTransaction
{
    [BsonIgnoreExtraElements]
    public class TransactionResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }
        public string PartnerTransaction { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public double MobileNetworkFee { get; set; }
        public string Desc { get; set; }
        public string PayMethod { get; set; }
        public string Title { get; set; }
        public string SourceData { get; set; }
        public string Status { get; set; }
        public DateTime? SuccessDate { get; set; }
        public string BillStatus { get; set; }
        public string BillPhoneNumber { get; set; }
        public string BillIdCard { get; set; }
        public IEnumerable<BillStepHistoriesResponse> BillStepHistories { get; set; }
        public PayMeDataResponse PaymeResponse { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public SaleDto SaleInfo { get; set; }
        public TeamLeadDto TeamLeadInfo { get; set; }
        public PosInfoDto PosInfo { get; set; }
        public SaleChanelDto SaleChanelInfo { get; set; }
    }
    public class PayMeDataResponse
    {
        public string URL { get; set; }
        public string Transaction { get; set; }
    }
    public class BillStepHistoriesResponse
    {
        public string Status { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
