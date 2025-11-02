using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class MafcGroupDocumentDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Mandatory { get; set; }
        public bool HasAlternate { get; set; }
        public bool Locked { get; set; }
        public IEnumerable<MafcDocumentUploadDto> Documents { get; set; }
    }

    public class MafcDocumentUploadDto
    {
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public string InputDocUid { get; set; }
        public string MapBpmVar { get; set; }
        public IEnumerable<MafcUploadedMediaDto> UploadedMedias { get; set; }
    }

    public class MafcUploadedMediaDto
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
}
