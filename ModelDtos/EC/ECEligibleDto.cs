using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECEligibleDto
    {
        [JsonPropertyName("identity_card_id")]
        [JsonProperty("identity_card_id")]
        public string IdentityCardId { get; set; }

        [JsonPropertyName("date_of_birth")]
        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("customer_name")]
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }

        [JsonPropertyName("issue_date")]
        [JsonProperty("issue_date")]
        public string IssueDate { get; set; }

        [JsonPropertyName("phone_number")]
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("issue_place")]
        [JsonProperty("issue_place")]
        public string IssuePlace { get; set; }

        [JsonPropertyName("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonPropertyName("dsa_agent_code")]
        [JsonProperty("dsa_agent_code")]
        public string DsaAgentCode { get; set; }

        [JsonPropertyName("tem_province")]
        [JsonProperty("tem_province")]
        public string TemProvince { get; set; }

        [JsonPropertyName("profession")]
        [JsonProperty("profession")]
        public string Profession { get; set; }
    }

    public class ECRestEligibleDto : ECEligibleDto
    {
        [JsonPropertyName("request_id")]
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonPropertyName("channel")]
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("partner_code")]
        [JsonProperty("partner_code")]
        public string PartnerCode { get; set; }

        [JsonPropertyName("sales_code")]
        [JsonProperty("sales_code")]
        public string SalesCode { get; set; }
    }
}
