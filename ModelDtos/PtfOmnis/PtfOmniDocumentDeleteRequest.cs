using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniDocumentDeleteRequest
    {
        [Required]
        public string CaseId { get; set; }

        [Required]
        public IEnumerable<string> DocIds { get; set; }
    }
}
