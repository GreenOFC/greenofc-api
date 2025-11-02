using Newtonsoft.Json;
using System;

namespace _24hplusdotnetcore.ModelResponses.MC
{
    public class SendOtpResponse
    {
        [JsonProperty("returnCode")]
        public string ReturnCode { get; set; }

        [JsonProperty("returnMes")]
        public string ReturnMes { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
        public string InforScore { get; set; }
        public string Score { get; set; }
        public string SourceScore { get; set; }
        public string VerifyInfo { get; set; }
    }
}