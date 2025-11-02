using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class GetCimbResourceRestResponse
    {
        public string SystemCode { get; set; }
        public string Message { get; set; }
        public CimbResourceDataRestResponse Data { get; set; }
    }

    public class CimbResourceDataRestResponse
    {
        public IEnumerable<CimbResourceRestDto> Resources { get; set; }
    }

    public class CimbResourceRestDto
    {
        public string Type { get; set; }
        public IEnumerable<CimbResourceItemRestDto> Data { get; set; }
    }

    public class CimbResourceItemRestDto
    {
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string Vi { get; set; }
        public string En { get; set; }
    }
}
