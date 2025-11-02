using Newtonsoft.Json;
using System;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniDocumentListResponse
    {
        [JsonProperty("doc_id")]
        public string DocId { get; set; }

        [JsonProperty("case_id")]
        public string CaseId { get; set; }

        [JsonProperty("doc_type")]
        public string DocType { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }

        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authorID")]
        public string AuthorID { get; set; }

        [JsonProperty("fileURL")]
        public string FileURL { get; set; }

        [JsonProperty("createAt")]
        public DateTime? CreateAt { get; set; }

        [JsonProperty("priority")]
        public int? Priority { get; set; }

        [JsonProperty("is_delete")]
        public int? IsDelete { get; set; }

        [JsonProperty("time_delete")]
        public long? TimeDelete { get; set; }
    }
}
