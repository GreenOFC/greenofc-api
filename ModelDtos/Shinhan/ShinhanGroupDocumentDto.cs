using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    public class ShinhanGroupDocumentDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Mandatory { get; set; }
        public bool HasAlternate { get; set; }
        public bool Locked { get; set; }
        public IEnumerable<ShinhanDocumentUploadDto> Documents { get; set; }
    }

    public class ShinhanDocumentUploadDto
    {
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public string InputDocUid { get; set; }
        public string MapBpmVar { get; set; }
        public IEnumerable<ShinhanUploadedMediaDto> UploadedMedias { get; set; }
    }

    public class ShinhanUploadedMediaDto
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}
