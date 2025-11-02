using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class GetMcResquest: PagingRequest 
    {
        public string Status { get; set; }
        [JsonProperty("customername")]
        public string CustomerName { get; set; }
        public string ProductLine { get; set; }
        public string ReturnStatus { get; set; }
    }
}
