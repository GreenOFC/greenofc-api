using _24hplusdotnetcore.Common.Constants;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public class CIMBLoanStatusBadDataResponse
    {
        [JsonProperty("message")]
        public CIMBMessage Message { get; set; }

        [JsonProperty("response")]
        public CIMBResponse Response { get; set; }
    }

    public class CIMBAccountOpeningRequestStatuses
    {
        [JsonProperty("partnerRequestId")]
        public string PartnerRequestId { get; set; }

        [JsonProperty("accountOpeningRequestId")]
        public string AccountOpeningRequestId { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("isReapplyingAllowed")]
        public bool IsReapplyingAllowed { get; set; }

        [JsonProperty("finalApprovedAmount")]
        public decimal FinalApprovedAmount { get; set; }

        [JsonProperty("loanId")]
        public string LoanId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public string GetReson()
        {
            if (string.IsNullOrEmpty(Reason))
            {
                return Reason;
            }

            var reasons = Reason.Split(',')
                .Select(x => x.Trim())
                .Select(x =>
                {
                    var matched = Regex.Match(x, @"^(.+?)[ ]");
                    if (matched.Success && StatusMapping.CIMB_STATUS_MESSAGE_MAPPING.TryGetValue(matched.Groups[1].Value, out string errorMsg))
                    {
                        return $"{x}: {errorMsg}";
                    }
                    return x;
                });

            return string.Join("; ", reasons);
        }
    }

    public class CIMBLoanStatusSuccessDataResponse
    {
        [JsonProperty("accountOpeningRequestStatuses")]
        public CIMBAccountOpeningRequestStatuses[] CIMBAccountOpeningRequestStatuses { get; set; }
    }

    public class CIMBLoanStatusBadResponse : CIMBBaseResponse
    {
        [JsonProperty("data")]
        public CIMBLoanStatusBadDataResponse Data { get; set; }
    }

    public class CIMBLoanStatusSuccessResponse : CIMBBaseResponse
    {
        [JsonProperty("data")]
        public CIMBLoanStatusSuccessDataResponse Data { get; set; }
    }
}
