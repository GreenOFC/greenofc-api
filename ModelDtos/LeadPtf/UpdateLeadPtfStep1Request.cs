namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class UpdateLeadPtfStep1Request: IUpdateLeadPtf, IUpdateLeadPtfPersonal
    {
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        public LeadPtfPersonalDto Personal { get; set; }
    }
}
