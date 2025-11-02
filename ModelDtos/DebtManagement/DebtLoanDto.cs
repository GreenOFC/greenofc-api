using System;

namespace _24hplusdotnetcore.ModelDtos.DebtManagement
{
    public class DebtLoanDto
    {
        public DateTime DisbursementDate { get; set; }
        public string Term { get; set; }
        public string Period { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string Amount { get; set; }
    }
}
