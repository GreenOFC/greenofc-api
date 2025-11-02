namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class GetLeadPtfRequest: PagingRequest
    {
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string ReturnStatus { get; set; }
        public string ProductLine { get; set; }
    }
}
