using Refit;

namespace _24hplusdotnetcore.ModelDtos.History
{
    public class GetHistoryRequest : PagingRequest
    {

        [AliasAs("customerId")]
        public string CustomerId { get; set; }
    }
}
