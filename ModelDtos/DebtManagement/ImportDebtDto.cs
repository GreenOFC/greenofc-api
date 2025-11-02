using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using System;


namespace _24hplusdotnetcore.ModelDtos.DebtManagement
{
    public class ImportDebtDto
    {
        [Column(1)]
        public string ContractCode { get; set; }
        [Column(2)]
        public string GreenType { get; set; }
        [Column(3)]
        public string Name { get; set; }
        [Column(4)]
        public string IdCard { get; set; }
        [Column(5)]
        public string Phone { get; set; }
        [Column(6)]
        [DateFormat(DateFormatValue.UnitedKingdomFormat)]
        public string DisbursementDate { get; set; }
        [Column(7)]
        public string Term { get; set; }
        [Column(8)]
        [DateFormat(DateFormatValue.ShortFormat)]
        public string Period { get; set; }
        [Column(9)]
        [DateFormat(DateFormatValue.UnitedKingdomFormat)]
        public string PaymentDueDate { get; set; }
        [Column(10)]
        public string Amount { get; set; }
        [Column(11)]
        public string Code { get; set; }
        [Column(12)]
        public string ActualUpdatedDate { get; set; }
    }
}
