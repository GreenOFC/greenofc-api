using _24hplusdotnetcore.Common.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class GreenTypeLeadSourceMapping
    {
        public static readonly ReadOnlyDictionary<string, LeadSourceType> GREEN_TYPE_LEAD_SOURCE =
            new ReadOnlyDictionary<string, LeadSourceType>(new Dictionary<string, LeadSourceType>() {
                { GreenType.GreenA,  LeadSourceType.MA},
                { GreenType.GreenC,  LeadSourceType.MC},
                { GreenType.GreenE,  LeadSourceType.Shinhan},
                { GreenType.GreenG,  LeadSourceType.CIMB},
                { GreenType.GreenD,  LeadSourceType.EC},
                { GreenType.GreenP,  LeadSourceType.PTF},
                { GreenType.GreenF88,  LeadSourceType.F88},
            });
    }
}
