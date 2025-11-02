namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcStep2Request
    {
        public LeadEcAddressDto ResidentAddress { get; set; }
        public LeadEcAddressDto TemporaryAddress { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
    }
}
