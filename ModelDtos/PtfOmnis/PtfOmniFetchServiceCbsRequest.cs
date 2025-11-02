using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniFetchServiceCbsRequest
    {
        [Required]
        public IEnumerable<string> Cif { get; set; }
    }
}
