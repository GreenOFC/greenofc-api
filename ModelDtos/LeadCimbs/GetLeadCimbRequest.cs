using Newtonsoft.Json;
using System;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class GetLeadCimbRequest: PagingRequest 
    {
        public string Status { get; set; }
        [JsonProperty("customername")]
        public string CustomerName { get; set; }
        public string ReturnStatus { get; set; }
    }
}
