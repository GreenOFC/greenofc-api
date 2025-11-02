using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadVbis
{
    [BsonIgnoreExtraElements]
    public class LeadVbiAddressDto
    {
        [Required]
        public DataConfigDto Province { get; set; }
    }
}
