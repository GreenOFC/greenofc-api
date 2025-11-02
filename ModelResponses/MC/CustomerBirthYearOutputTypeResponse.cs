using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelResponses.MC
{
    public class CustomerBirthYearOutputTypeResponse
    {
        [JsonPropertyName("OutputType")]
        [JsonProperty("outputType")]
        public string OutputType { get; set; }

        [JsonPropertyName("OutputValue")]
        [JsonProperty("outputValue")]
        public string OutputValue { get; set; }
    }
}
