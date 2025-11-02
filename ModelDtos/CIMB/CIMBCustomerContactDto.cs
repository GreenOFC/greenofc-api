using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBCustomerContactDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}