using System;

namespace _24hplusdotnetcore.Models.DebtManagement
{
    public class DebtLoan
	{
        public DateTime? DisbursementDate { get; set; }
		public string Term { get; set; }
		public DateTime? Period { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string Amount { get; set; }
    }
}
