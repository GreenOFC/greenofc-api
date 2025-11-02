namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcWorkingStep4Request
    {
        public string Job { get; set; }
        public string JobId { get; set; }
        public string CompanyName { get; set; }
        public LeadEcAddressDto CompanyAddress { get; set; }
        public string TaxCode { get; set; }
    }
}
