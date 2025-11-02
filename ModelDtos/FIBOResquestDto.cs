using System.ComponentModel.DataAnnotations;
using Refit;

namespace _24hplusdotnetcore.ModelDtos
{
    public class FIBOResquestDto
    {
        [AliasAs("contactName")]
        public string ContactName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [AliasAs("phone")]
        public string Phone { get; set; }
        [AliasAs("cmnd")]
        public string Cmnd { get; set; }
        [AliasAs("province")]
        public string Province { get; set; }
        [AliasAs("leadSource")]
        public string LeadSource { get; set; }
    }
}
