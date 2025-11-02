using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class UpdateDocumentLeadPtfRequest: IUpdateLeadPtf, ISubmitLeadPtf
    {
        public bool IsSubmit { get; set; }
        [Required]
        public IEnumerable<LeadPtfGroupDocumentDto> Documents { get; set; }
    }
}
