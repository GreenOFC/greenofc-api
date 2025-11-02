using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class UpdateMafcStep6Request
    {
        public IEnumerable<MafcGroupDocumentDto> Documents { get; set; }
        public IEnumerable<MafcGroupDocumentDto> ReturnDocuments { get; set; }
        public string CaseNote { get; set; }
        public MafcUploadedMediaDto RecordFile { get; set; }
        public string Status { get; set; }
    }
}
