using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbGroupDocumentDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Mandatory { get; set; }
        public bool HasAlternate { get; set; }
        public bool Locked { get; set; }
        public IEnumerable<CimbDocumentUploadDto> Documents { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CimbDocumentUploadDto
    {
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public string InputDocUid { get; set; }
        public string MapBpmVar { get; set; }
        public IEnumerable<CimbUploadedMediaDto> UploadedMedias { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class CimbUploadedMediaDto
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}
