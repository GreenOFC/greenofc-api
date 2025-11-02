using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class GetLeadEcRequest: PagingRequest 
    {
        public string Status { get; set; }
        [JsonProperty("customername")]
        public string CustomerName { get; set; }
        public string ReturnStatus { get; set; }
    }
}
