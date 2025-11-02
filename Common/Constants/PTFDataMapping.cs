using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _24hplusdotnetcore.Common.Constants
{
    public static class PTFDataMapping
    {
        public static readonly ReadOnlyDictionary<string, int> QUEUE_NAME =
            new ReadOnlyDictionary<string, int>(new Dictionary<string, int>() {
                {"PTF_Request_Initiate", 1},
                {"PTF_Data_Entry", 3},
            });

    }
}