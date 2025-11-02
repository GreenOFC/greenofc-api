using System;

namespace _24hplusdotnetcore.ModelResponses.DebtManagement
{
    public class DebtLoanResponse
    {
        public DateTime? DisbursementDate { get; set; }
        public string Term { get; set; }
        public DateTime? Period { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string Amount { get; set; }
    }
}
