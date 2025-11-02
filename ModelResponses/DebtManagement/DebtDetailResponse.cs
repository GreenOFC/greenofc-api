using System;

namespace _24hplusdotnetcore.ModelResponses.DebtManagement
{
    public class DebtDetailResponse
    {
        public string Id { get; set; }
        public string ContractCode { get; set; }
        public string GreenType { get; set; }
        public DebtPersonalResponse Personal { get; set; }
        public DebtLoanResponse Loan { get; set; }
        public DebtSaleInfoResponse SaleInfo { get; set; }
        public DebtSaleInfoResponse ModifierInfo { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
        public string ActualUpdatedDate { get; set; }
        public string Type { get; set; }
        public int? NumberOverDueDate { get; set; }
        public DateTime? OverDueDate { get; set; }
        public int RowNumber { get; set; }
    }
}
