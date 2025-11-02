using Newtonsoft.Json;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScoringTrustingSocialFetchFraudScoreRestRequest
    {
        public string PhoneNumber { get; set; }

        [JsonProperty("id_type")]
        public string IdType { get; set; }

        [JsonProperty("id_value")]
        public string IdValue { get; set; }

        [JsonProperty("frequent_contacts")]
        public IEnumerable<string> FrequentContacts { get; set; }
    }
}
