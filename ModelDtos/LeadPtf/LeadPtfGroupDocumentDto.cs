using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfGroupDocumentDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public bool Mandatory { get; set; }
        public bool HasAlternate { get; set; }
        public bool Locked { get; set; }
        public IEnumerable<LeadPtfDocumentUploadDto> Documents { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LeadPtfDocumentUploadDto
    {
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public string InputDocUid { get; set; }
        public string MapBpmVar { get; set; }
        public IEnumerable<LeadPtfUploadedMediaDto> UploadedMedias { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LeadPtfUploadedMediaDto
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public string DocumentId { get; set; }
    }
}
