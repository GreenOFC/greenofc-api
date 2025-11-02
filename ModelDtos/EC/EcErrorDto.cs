using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class EcErrorDto
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public EcErrorDetailDto Error { get; set; }
    }

    public class EcErrorDetailDto
    {
        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}
