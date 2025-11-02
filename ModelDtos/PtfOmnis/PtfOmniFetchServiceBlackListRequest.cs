using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniFetchServiceBlackListRequest
    {
        public IEnumerable<string> IdDocumentNo { get; set; }

        public string FrbDocumentNo { get; set; }

        public IEnumerable<string> PhoneNumber { get; set; }
    }
}
