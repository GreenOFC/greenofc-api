using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class UpdateLeadCimbStep4Request
    {
        [Required]
        public IEnumerable<CimbGroupDocumentDto> Documents { get; set; }
    }
}
