using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public class CIMBSubmitDataResponse
    {
        [JsonProperty("loanId")]
        public string LoanID { get; set; }

        [JsonProperty("message")]
        public CIMBMessage Message { get; set; }

        [JsonProperty("response")]
        public CIMBResponse Response { get; set; }
    }

    public class CIMBSubmitResponse : CIMBBaseResponse
    {
        [JsonProperty("data")]
        public CIMBSubmitDataResponse Data { get; set; }
    }
}
