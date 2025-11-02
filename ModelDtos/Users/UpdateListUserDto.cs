using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UpdateListUserDto
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("teamLead")]
        public string TeamLead { get; set; }

        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("posName")]
        public string PosName { get; set; }
        [JsonProperty("ecDsaCode")]
        public string EcDsaCode { get; set; }
    }
}
