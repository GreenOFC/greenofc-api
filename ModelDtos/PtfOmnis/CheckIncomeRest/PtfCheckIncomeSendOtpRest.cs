using System;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeRest
{
    public class PtfCheckIncomeSendOtpRestRequest
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public string PhoneNumber { get; set; }
    }
    public class PtfCheckIncomeSendOtpRestResponse
    {
        [JsonProperty("otpId")]
        public int OtpId { get; set; }
        [JsonProperty("telco_code")]
        public string TelcoCode { get; set; }
    }

}
