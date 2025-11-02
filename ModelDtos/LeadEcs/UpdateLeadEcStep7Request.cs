using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    public class UpdateLeadEcStep7Request
    {
        public bool IsSubmit { get; set; }
        [Required]
        public IEnumerable<LeadEcGroupDocumentDto> Documents { get; set; }
    }
}
