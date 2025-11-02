using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniDocumentUploadRequest
    {
        public string CaseId { get; set; }

        [Required]
        public string DocumentCategory { get; set; }

        [Required]
        public string DocumentType { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
