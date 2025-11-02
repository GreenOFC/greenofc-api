using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCCheckCustomerRestResponse
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Partner { get; set; }
        public int? StatusNumber { get; set; }
    }
    public class MAFCCheckCustomerV3RestResponse
    {
        [JsonProperty("cmnd")]
        public string CMND { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("taxCode")]
        public string TaxCode { get; set; }
        [JsonProperty("partner")]
        public string Partner { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("saleCode")]
        public string SaleCode { get; set; }
        [JsonProperty("statusNumber")]
        public int? StatusNumber { get; set; }
        public string GetMessage()
        {
            string result = "";
            switch (StatusNumber)
            {
                case 0:
                    result = "KH hợp lệ để nộp hồ sơ vay tại MAFC";
                    break;
                case 1:
                    result = "KH không hợp lệ để nộp hồ sơ vay tại MAFC (Blacklist mã số thuế)";
                    break;
                case 2:
                    result = "KH không hợp lệ để nộp hồ sơ vay tại MAFC (Blacklist)";
                    break;
                default:
                    result = "KH không hợp lệ để nộp hồ sơ vay tại MAFC (Bị trùng hoặc có nợ xấu)";
                    break;
            }
            return result;
        }
    }
}
