using Newtonsoft.Json;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBLoanStatusDto
    {
        [JsonProperty("loanIds")]
        public IEnumerable<string> LoanIds { get; set; }
    }
}
