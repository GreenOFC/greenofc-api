using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniFetchServiceCifRequest
    {
        public IEnumerable<string> IdDocumentNo { get; set; }
    }
}
