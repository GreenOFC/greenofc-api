using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class UpdateMcLeadStep2Request
    {
        public IEnumerable<McGroupDocumentDto> Documents { get; set; }
        public string Status { get; set; }
    }
}
