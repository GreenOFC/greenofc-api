using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class UpdateMcStep5Request
    {
        public IEnumerable<McGroupDocumentDto> Documents { get; set; }
        public IEnumerable<McGroupDocumentDto> ReturnDocuments { get; set; }
        public string CaseNote { get; set; }
        public McUploadedMediaDto RecordFile { get; set; }
        public string Status { get; set; }
    }
}
