using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.MCDebt)]
    public class MCDebt : BaseEntity
    {
        public string AppNumber { get; set; }
        public string CustomerName { get; set; }
        public string CitizenId { get; set; }
        public string ContractNumber { get; set; }
        public bool HasInsurrance { get; set; }
        public string AccountNumber { get; set; }
        public string AtBank { get; set; }
        public string DisbursementChannel { get; set; }
        public string ChannelName { get; set; }
        public bool HasCourier { get; set; }
        public string LoanApprovedAmt { get; set; }
        public string LoanApprovedTenor { get; set; }

        public string ContractCode { get; set; }
        public string Phone { get; set; }
        public string LoanCategory { get; set; }
        public string LoanProduct { get; set; }


        public string AccountStatus { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DisbursementDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NextPaymentDate { get; set; }
        public int CurrentDebtPeriod { get; set; }
        public string TotalLoanAmt { get; set; }
        public string MonthlyPayment { get; set; }
        public bool IsFollowed { get; set; }

        public MCDebt()
        {
            DisbursementDate = DateTime.Now;
            CurrentDebtPeriod = 1;
            IsFollowed = true;
            NextPaymentDate = DisbursementDate.AddMonths(1);
        }
    }
}
