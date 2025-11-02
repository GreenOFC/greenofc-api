using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class Scoring3PRequest
    {
        [JsonPropertyName("primaryPhone")]
        [JsonProperty("primaryPhone")]
        public string PrimaryPhone { get; set; }

        [JsonPropertyName("nationalId")]
        [JsonProperty("nationalId")]
        public string NationalId { get; set; }

        [JsonPropertyName("typeScore")]
        [JsonProperty("typeScore")]
        public string TypeScore { get; set; }

        [JsonPropertyName("verificationCode")]
        [JsonProperty("verificationCode")]
        public string VerificationCode { get; set; }
    }

    public class Scoring3PRestRequest : Scoring3PRequest
    {
        [JsonPropertyName("userName")]
        [JsonProperty("userName")]
        public string UserName { get; set; }

    }
}