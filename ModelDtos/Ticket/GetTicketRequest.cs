namespace _24hplusdotnetcore.ModelDtos.Ticket
{
    public class GetTicketRequest : PagingRequest
    {
        public string Project { get; set; } = "";
        public string Status { get; set; } = "";

    }
}
