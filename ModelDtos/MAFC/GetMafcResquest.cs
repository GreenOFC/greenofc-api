using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{

    public class GetMafcResquest : PagingRequest 
    {
        public string Status { get; set; }
        [JsonProperty("customername")]
        public string CustomerName { get; set; }
        public string ProductLine { get; set; }
        public string ReturnStatus { get; set; }
    }
    public class GetOldAppMafcResquest : PagingRequest 
    {
        [JsonProperty("customername")]
        public string CustomerName { get; set; }
    }
}
