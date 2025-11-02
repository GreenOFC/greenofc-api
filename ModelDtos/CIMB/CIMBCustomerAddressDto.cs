using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBCustomerAddressDto
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("ward")]
        public string Ward { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}
