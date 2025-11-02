using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class GetCimbResourceRestRequest
    {
        public IEnumerable<string> RequestingResources { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
