using _24hplusdotnetcore.Common.Attributes;
using System;

namespace _24hplusdotnetcore.ModelResponses.DebtManagement
{
    public class ExportDebtManagement
    {
        [Export(ExportName = "contractCode")]
        public string ContractCode { get; set; }

        [Export(ExportName = "greenType")]
        public string GreenType { get; set; }

        [Export(ExportName = "customer")]
        public string Name { get; set; }

        [Export(ExportName = "idCard")]
        public string IdCard { get; set; }

        [Export(ExportName = "phone")]
        public string Phone { get; set; }

        [Export(ExportName = "disbursementDate")]
        public DateTime? DisbursementDate { get; set; }

        [Export(ExportName = "term")]
        public string Term { get; set; }

        [Export(ExportName = "period")]
        public DateTime? Period { get; set; }

        [Export(ExportName = "paymentDueDate")]
        public DateTime? PaymentDueDate { get; set; }

        [Export(ExportName = "amount")]
        public decimal? Amount { get; set; }

        [Export(ExportName = "saleCode")]
        public string Code { get; set; }

        [Export(ExportName = "actualUpdatedDate")]
        public string ActualUpdatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int RowNumber { get; set; }
    }
}
