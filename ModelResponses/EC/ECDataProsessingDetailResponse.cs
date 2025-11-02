using Newtonsoft.Json.Linq;
using System;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECDataProsessingDetailResponse
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string PayLoad { get; set; }
        public string Response { get; set; }
        public object FormatedPayload => PayLoad == null ? null : JObject.Parse(PayLoad);
        public object FormatedResponse => Response == null ? null : JObject.Parse(Response);
    }
}