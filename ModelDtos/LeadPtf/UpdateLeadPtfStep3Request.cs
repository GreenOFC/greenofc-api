namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class UpdateLeadPtfStep3Request: IUpdateLeadPtf
    {
        public LeadPtfLoanDto Loan { get; set; }
        public LeadPtfDisbursementInformationDto DisbursementInformation { get; set; }
    }
}
