using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    public class GetShinhanResquest: PagingRequest 
    {
        public string Status { get; set; }
        [JsonProperty("customername")]
        public string CustomerName { get; set; }
        public string ProductLine { get; set; }
        public string ReturnStatus { get; set; }
    }
}
