using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    public class LeadHome: LeadSource
    {
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public LeadHomeAddress TemporaryAddress { get; set; }
    }

    public class LeadHomeAddress
    {
        public DataConfigModel Province { get; set; }
        public DataConfigModel District { get; set; }
        public DataConfigModel Ward { get; set; }
        public string Street { get; set; }

        public string GetFullAddress()
        {
            return string.Join(", ", new List<string> { Street, Ward?.Value, District?.Value, Province?.Value }
                .Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}
