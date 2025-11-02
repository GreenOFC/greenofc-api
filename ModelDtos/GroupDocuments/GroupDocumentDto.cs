using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.GroupDocuments
{
    [BsonIgnoreExtraElements]
    public class GroupDocumentDto
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }
        
        public bool Mandatory { get; set; }
        
        public bool HasAlternate { get; set; }
        
        public bool Locked { get; set; }
        
        public IEnumerable<DocumentUploadDto> Documents { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class DocumentUploadDto
    {
        public int Id { get; set; }

        public string DocumentCode { get; set; }

        public string DocumentName { get; set; }

        public string InputDocUid { get; set; }

        public string MapBpmVar { get; set; }

        public IEnumerable<UploadedMediaDto> UploadedMedias { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UploadedMediaDto
    {
        public string OriginalName { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Uri { get; set; }
    }
}
