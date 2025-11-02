using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniCheckValidLoanRequest
    {
        public string FamilyBookNo { get; set; }

        public IEnumerable<string> IdCards { get; set; }

        public IEnumerable<string> Phones { get; set; }
    }
}
