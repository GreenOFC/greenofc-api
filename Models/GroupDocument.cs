using _24hplusdotnetcore.Common.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    public class GroupDocument
    {
        [KeyAuditing]
        [JsonProperty("groupId")]
        public int GroupId { get; set; }
        [JsonProperty("groupName")]
        public string GroupName { get; set; }
        [JsonProperty("groupCode")]
        public string GroupCode { get; set; }
        [JsonProperty("mandatory")]
        public bool Mandatory { get; set; }
        [JsonProperty("hasAlternate")]
        public bool HasAlternate { get; set; }
        [JsonProperty("locked")]
        public bool Locked { get; set; }
        [JsonProperty("documents")]
        public IEnumerable<DocumentUpload> Documents { get; set; }
    }

    public class DocumentUpload
    {
        [KeyAuditing]
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("documentCode")]
        public string DocumentCode { get; set; }
        [JsonProperty("documentName")]
        public string DocumentName { get; set; }
        [JsonProperty("inputDocUid")]
        public string InputDocUid { get; set; }
        [JsonProperty("mapBpmVar")]
        public string MapBpmVar { get; set; }
        [JsonProperty("uploadedMedias")]
        public IEnumerable<UploadedMedia> UploadedMedias { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UploadedMedia
    {
        [KeyAuditing]
        [JsonProperty("originalName")]
        public string OriginalName { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        public string DocumentId { get; set; }
    }
}
