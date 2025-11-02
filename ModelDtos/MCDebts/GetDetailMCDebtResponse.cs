using _24hplusdotnetcore.ModelDtos.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MCDebts
{
    [BsonIgnoreExtraElements]
    public class GetDetailMCDebtResponse
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int AppNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerIdCard { get; set; }
        public decimal? LoanApprovedAmt { get; set; }
        public string LoanApprovedTenor { get; set; }
        public string ContractCode { get; set; }
        public string ContractNumber { get; set; }
        public string LoanCategory { get; set; }
        public string LoanProduct { get; set; }
        public bool? HasInsurrance { get; set; }
        public bool? HasCourier { get; set; }
        public string AtBank { get; set; }
        public string AccountNumber { get; set; }
        public string AccountStatus { get; set; }
        public string DisbursementChannel { get; set; }
        public string ChannelName { get; set; }
        public DateTime DisbursementDate { get; set; }
        public int CurrentDebtPeriod { get; set; }
        public string TotalLoanAmt { get; set; }
        public string MonthlyPayment { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public bool IsFollowed { get; set; }
        public int NumberOfDaysOverdue => DateTime.Now > NextPaymentDate ? DateTime.Now.Subtract(NextPaymentDate).Days : 0;
        public DateTime CreatedDate { get; set; }
        public IEnumerable<SaleInfoResponse> SaleInfo { get; set; }
    }
}
