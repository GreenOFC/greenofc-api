namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class LeadCimbLoanInfomationResponse
    {
        public LeadCimbLoanInfomationResponse(int numberOfMonth, double interestRatePerYear, double monthlyPaymentAmount)
        {
            NumberOfMonth = numberOfMonth;
            InterestRatePerYear = interestRatePerYear;
            MonthlyPaymentAmount = monthlyPaymentAmount;
        }

        public int NumberOfMonth { get; set; }
        public double InterestRatePerYear { get; set; }
        public double MonthlyPaymentAmount { get; set; }
    }
}
