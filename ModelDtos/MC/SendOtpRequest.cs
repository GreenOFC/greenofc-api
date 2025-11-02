using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class SendOtpRequest
    {
        [JsonPropertyName("requested_msisdn")]
        [JsonProperty("requested_msisdn")]
        public string RequestedMsisdn { get; set; }
        public string IdCard { get; set; }

        [JsonPropertyName("typeScore")]
        [JsonProperty("typeScore")]
        public string TypeScore { get; set; }
        [JsonPropertyName("transactionId")]
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
    public class SendOtpRestRequest
    {
        [JsonPropertyName("requested_msisdn")]
        [JsonProperty("requested_msisdn")]
        public string RequestedMsisdn { get; set; }

        [JsonPropertyName("typeScore")]
        [JsonProperty("typeScore")]
        public string TypeScore { get; set; }
    }
}
