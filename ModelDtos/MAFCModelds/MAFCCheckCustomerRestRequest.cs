using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCCheckCustomerRestRequest
    {
        [JsonProperty("searchVal")]
        public string SearchVal { get; set; }
        [JsonProperty("cmnd")]
        public string CMND { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("taxCode")]
        public string TaxCode { get; set; }
        [JsonProperty("partner")]
        public string Partner { get; set; }
    }
    public class MAFCCheckCustomerV3RestRequest
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
    }
}
