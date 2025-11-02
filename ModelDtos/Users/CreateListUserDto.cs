using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class CreateListUserRequest
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
        
        [JsonProperty("idCard")]
        public string IdCard { get; set; }

        [JsonProperty("userPassword")]
        public string UserPassword { get; set; }

        [JsonProperty("teamLead")]
        public string TeamLead { get; set; }

        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("mafcCode")]
        public string MAFCCode { get; set; }
        [JsonProperty("posName")]
        public string PosName { get; set; }
        [JsonProperty("ecDsaCode")]
        public string EcDsaCode { get; set; }
    }
}
