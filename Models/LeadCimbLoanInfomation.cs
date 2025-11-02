using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using Microsoft.VisualBasic;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.LeadCimbLoanInfomation)]
    public class LeadCimbLoanInfomation: BaseDocument
    {
        public string PackageSize { get; set; }
        public double MinAmount { get; set; }
        public double? MaxAmount { get; set; }
        public IEnumerable<LeadCimbLoanInfomationDetail> Details { get; set; }
    }

    public class LeadCimbLoanInfomationDetail
    {
        public int NumberOfMonth { get; set; }
        public double InterestRatePerYear { get; set; }
        public double InterestRatePerMonth => InterestRatePerYear / 12;

        public double MonthlyPaymentAmount(double amount)
        {
            return Financial.Pmt(InterestRatePerMonth / 100, NumberOfMonth, (-1) * amount);
        }
    }
}
