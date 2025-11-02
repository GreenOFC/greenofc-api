using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBEmployment
    {
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("employmentStatus")]
        public string EmploymentStatus { get; set; }
    }
}