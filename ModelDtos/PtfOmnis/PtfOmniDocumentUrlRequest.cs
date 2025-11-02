using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniDocumentUrlRequest
    {
        [Required]
        public string CaseId { get; set; }

        [Required]
        public string DocID { get; set; }

        [Required]
        public string DocType { get; set; }
    }
}
