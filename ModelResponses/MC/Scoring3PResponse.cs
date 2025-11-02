using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.MC
{
    public class Scoring3PResponse
    {
        [JsonProperty("returnCode")]
        public string ReturnCode { get; set; }

        [JsonProperty("returnMes")]
        public string ReturnMes { get; set; }

        [JsonProperty("inforScore")]
        public string InforScore { get; set; }

        [JsonProperty("sourceScore")]
        public string SourceScore { get; set; }

        [JsonProperty("verifyInfo")]
        public string VerifyInfo { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productCode")]
        public string ProductCode { get; set; }
    }
}
