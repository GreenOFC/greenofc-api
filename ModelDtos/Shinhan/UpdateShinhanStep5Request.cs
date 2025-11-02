using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    public class UpdateShinhanStep5Request
    {
        public IEnumerable<ShinhanGroupDocumentDto> Documents { get; set; }
        public IEnumerable<ShinhanGroupDocumentDto> ReturnDocuments { get; set; }
        public string CaseNote { get; set; }
        public ShinhanUploadedMediaDto RecordFile { get; set; }
        public string Status { get; set; }
    }
}
