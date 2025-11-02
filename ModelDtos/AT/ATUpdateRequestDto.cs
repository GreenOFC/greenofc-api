using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Refit;

namespace _24hplusdotnetcore.ModelDtos.AT
{
    public class ATUpdateRequestDto
    {
        [JsonProperty("transaction_id")]
        public string transaction_id { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("rejected_reason")]
        public string rejected_reason { get; set; }
        [JsonProperty("items")]
        public IEnumerable<ATItemDto> items { get; set; }
    }

    public class ATItemDto
    {
        [JsonProperty("id")]
        public string id { get; set; } = "24hplus";
        [JsonProperty("status")]
        public int status { get; set; }
        [JsonProperty("extra")]
        public ATExtraDto extra { get; set; }
    }

    public class ATExtraDto
    {
        [JsonProperty("rejected_reason")]
        public string rejected_reason { get; set; }
    }
}
