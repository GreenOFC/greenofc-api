namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class UpdateMafcStep3WebRequest
    {
        public MafcLoanDto Loan { get; set; }
        public MafcWorkingDto Working { get; set; }
        public MafcBankInfoDto BankInfo { get; set; }
        public MafcOtherInfoDto OtherInfo { get; set; }
    }
}
