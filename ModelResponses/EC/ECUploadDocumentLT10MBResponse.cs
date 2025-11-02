using Newtonsoft.Json;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelResponses.EC
{
    public class ECUploadDocumentLT10MBResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("listDocCollecting")]
        public List<ListDocCollecting> ListDocCollectings { get; set; }
    }

    public class ListDocCollecting
    {
        [JsonProperty("docName")]
        public string DocName { get; set; }

        [JsonProperty("docId")]
        public string DocId { get; set; }
    }
}
