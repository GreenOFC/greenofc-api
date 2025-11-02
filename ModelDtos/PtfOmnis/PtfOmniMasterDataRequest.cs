using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniMasterDataRequest
    {
        [Required]
        public IEnumerable<string> Types { get; set; }

        public string ParentType { get; set; }

        public string ParentValue { get; set; }
    }
}
