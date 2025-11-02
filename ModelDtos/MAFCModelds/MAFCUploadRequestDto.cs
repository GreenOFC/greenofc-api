using System.Collections.Generic;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCUploadRequestDto
    {
        [JsonProperty("warning")]
        public string Warning { get; set; }
        [JsonProperty("warning_msg")]
        public string Warning_msg { get; set; }
        [JsonProperty("appid")]
        public int Appid { get; set; }
        [JsonProperty("salecode")]
        public string Salecode { get; set; }
        [JsonProperty("usersname")]
        public string Usersname { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("files")]
        public IEnumerable<MAFCFileUploadRequestDto> Files { get; set; }

    }
    public class MAFCFileUploadRequestDto
    {
        [JsonProperty("documentCode")]
        public string DocumentCode { get; set; }
        [JsonProperty("fileName")]
        public string FileName { get; set; }

    }
}
