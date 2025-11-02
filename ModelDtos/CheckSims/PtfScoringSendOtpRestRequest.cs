using System;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringSendOtpRestRequest
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
