using Newtonsoft.Json.Linq;
using System;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class DataMCProcessingResponse
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public JObject PayLoad { get; set; }
        public JObject Response { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
