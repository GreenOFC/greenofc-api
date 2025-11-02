using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringVerifyOtpVnptRestRequest
    {
        [JsonProperty("otp")]
        public string Otp { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
