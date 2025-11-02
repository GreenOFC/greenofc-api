namespace _24hplusdotnetcore.ModelDtos.Ticket
{
    public class GetReportTicketRequest : PagingRequest
    {
        public string Project { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
