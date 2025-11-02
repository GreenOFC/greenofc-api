using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class McGroupDocumentDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Mandatory { get; set; }
        public bool HasAlternate { get; set; }
        public bool Locked { get; set; }
        public IEnumerable<McDocumentUploadDto> Documents { get; set; }
    }

    public class McDocumentUploadDto
    {
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public string InputDocUid { get; set; }
        public string MapBpmVar { get; set; }
        public IEnumerable<McUploadedMediaDto> UploadedMedias { get; set; }
    }

    public class McUploadedMediaDto
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}
