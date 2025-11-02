using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class UpdateLeadPtfStep2Request: IUpdateLeadPtf
    {
        public LeadPtfWorkingDto Working { get; set; }
        public IEnumerable<LeadPtfRefereeDto> Referees { get; set; }
    }
}
