using Newtonsoft.Json;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECProductListResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public IEnumerable<ECProductListDataResponse> Data { get; set; }
    }

    public class ECProductListDataResponse
    {
        [JsonProperty("employee_type")]
        public string EmployeeType { get; set; }
        [JsonProperty("employee_description_v1")]
        public string EmployeeDescriptionVi { get; set; }
        [JsonProperty("employee_description_en")]
        public string EmployeeDescriptionEn { get; set; }

        [JsonProperty("product_list")]
        public IEnumerable<ECParentDocumentCollectingResponse> Products { get; set; }
    }

    public class ECParentDocumentCollectingResponse
    {
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("product_description")]
        public string ProductDescription { get; set; }
        [JsonProperty("loan_min_amount")]
        public decimal? LoanMinAmount { get; set; }
        [JsonProperty("loan_max_amount")]
        public decimal? LoanMaxAmount { get; set; }
        [JsonProperty("loan_min_tenor")]
        public decimal? LoanMinTenor { get; set; }
        [JsonProperty("loan_max_tenor")]
        public decimal? LoanMaxTenor { get; set; }
        [JsonProperty("interest_rate")]
        public decimal? InterestRate { get; set; }

        [JsonProperty("document_collecting")]
        public IEnumerable<ECChildDocumentCollectingResponse> Documents { get; set; }
    }

    public class ECChildDocumentCollectingResponse
    {
        [JsonProperty("bundle_name")]
        public string BundleName { get; set; }
        [JsonProperty("bundle_code")]
        public string BundleCode { get; set; }
        [JsonProperty("min_request")]
        public int? MinRequest { get; set; }

        [JsonProperty("doc_list")]
        public IEnumerable<ECDocumentList> DocumentItems { get; set; }
    }

    public class ECDocumentList
    {
        [JsonProperty("doc_description_vi")]
        public string DocDescriptionVi { get; set; }
        [JsonProperty("doc_description_en")]
        public string DocDescriptionEn { get; set; }
        [JsonProperty("doc_type")]
        public string DocType { get; set; }
        [JsonProperty("doc_format_request")]
        public string DocFormatRequest { get; set; }
    }
}
