using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    public class LeadVbi: LeadSource
    {
        public LeadVbi()
        {
            Status = LeadSourceStatus.Submit;
        }

        public string FullName { get; set; }

        public string IdCard { get; set; }

        public string Phone { get; set; }

        public string ExtraPhone { get; set; }

        public LeadVbiAddress TemporaryAddress { get; set; }

        [JsonConverter(typeof(LeadSourceStatus))]
        [BsonRepresentation(BsonType.String)]
        public LeadSourceStatus Status { get; set; }
    }

    public class LeadVbiAddress
    {
        public DataConfigModel Province { get; set; }

        public string GetFullAddress()
        {
            return Province?.Value ?? string.Empty;
        }
    }
}
