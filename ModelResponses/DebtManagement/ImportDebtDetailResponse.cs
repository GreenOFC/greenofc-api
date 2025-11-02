using System;

namespace _24hplusdotnetcore.ModelResponses.DebtManagement
{
    public class ImportDebtDetailResponse
    {
        public string ContractCode { get; set; }
        public string GreenType { get; set; }
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string DisbursementDate { get; set; }
        public string Term { get; set; }
        public string Period { get; set; }
        public string PaymentDueDate { get; set; }
        public string Amount { get; set; }
        public string Code { get; set; }
        public string ActualUpdatedDate { get; set; }
        public bool IsValid { get; set; } = true;
        public bool IsDuplicated { get; set; }
        public string Message { get; set; }
        public string OverDueDate { get; set; }
        public string NumberOverDueDate { get; set; }
        public string FirstReferee { get; set; }
        public string SecondReferee { get; set; }
    }
}
