using System;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeRest
{
    public class PtfCheckIncomeFetchRestRequest
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public string PhoneNumber { get; set; }
        public string IdCode { get; set; }
    }
    public class PtfCheckIncomeFetchRestResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("reqUserName")]
        public string ReqUserName { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("request_count_180")]
        public int RequestCount180 { get; set; }
        [JsonProperty("request_count_90")]
        public int RequestCount90 { get; set; }
        [JsonProperty("request_count_30")]
        public int RequestCount30 { get; set; }
        [JsonProperty("bank_count_30")]
        public int BankCount30 { get; set; }
        [JsonProperty("incomeScore")]
        public string IncomeScore { get; set; }
        [JsonProperty("verify")]
        public string Verify { get; set; }
        [JsonProperty("telco_code")]
        public string TelcoCode { get; set; }
        [JsonProperty("idCode")]
        public string IdCode { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("reqUser")]
        public string ReqUser { get; set; }
        [JsonProperty("reqBy")]
        public string ReqBy { get; set; }
        [JsonProperty("predictedIncome")]
        public string PredictedIncome { get; set; }
        [JsonProperty("isDelete")]
        public bool IsDelete { get; set; }
        [JsonProperty("exprireDate")]
        public DateTime? ExprireDate { get; set; }
        [JsonProperty("saveDate")]
        public DateTime? SaveDate { get; set; }
    }

}
