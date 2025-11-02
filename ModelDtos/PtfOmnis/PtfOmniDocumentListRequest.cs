using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniDocumentListRequest
    {
        [Required]
        public string CaseId { get; set; }

        public List<string> DocumentType { get; set; }

        public bool Active { get; set; }
    }
}
