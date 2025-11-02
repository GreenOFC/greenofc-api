namespace _24hplusdotnetcore.ModelDtos.DebtManagement
{
    public class CreateDebtDto
    {
        public string ContractCode { get; set; }
        public string GreenType { get; set; }
        public DebtPersonalDto Personal { get; set; }
        public DebtLoanDto Loan { get; set; }
        public DebtSaleInfoDto SaleInfo { get; set; }
    }
}
