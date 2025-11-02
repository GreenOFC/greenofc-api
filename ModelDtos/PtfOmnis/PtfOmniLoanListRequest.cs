using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniLoanListRequest
    {
        public int? Limit { get; set; }
        public int? Start { get; set; }
        public IEnumerable<PtfOmniLoanListCondition> Conditions { get; set; }
        public bool? Count { get; set; }
    }

    public class PtfOmniLoanListCondition
    {
        public string Key { get; set; }
        public IEnumerable<object> Value { get; set; }
    }
}
