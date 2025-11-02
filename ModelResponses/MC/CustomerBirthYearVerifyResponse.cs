using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelResponses.MC
{
    public class CustomerBirthYearVerifyResponse
    {
        public CustomerBirthYearVerifyResponse()
        {
            IsValid = false;
            ListValue = new List<CustomerBirthYearOutputTypeResponse>();
        }

        [JsonPropertyName("ReturnCode")]
        [JsonProperty("returnCode")]
        public int ReturnCode { get; set; }

        [JsonPropertyName("ReturnMes")]
        [JsonProperty("returnMes")]
        public string ReturnMes { get; set; }

        [JsonPropertyName("IsValid")]
        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("CustomerName")]
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("CustomerId")]
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonPropertyName("CurrentCustomerYear")]
        [JsonProperty("currentCustomerYear")]
        public int? CurrentCustomerYear { get; set; }

        [JsonPropertyName("ScalarValue")]
        [JsonProperty("scalarValue")]
        public string ScalarValue { get; set; }

        [JsonPropertyName("SultiValue")]
        [JsonProperty("sultiValue")]
        public string SultiValue { get; set; }

        [JsonPropertyName("ListValue")]
        [JsonProperty("listValue")]
        public List<CustomerBirthYearOutputTypeResponse> ListValue { get; set; }
    }
}
