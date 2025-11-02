using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadOkVays
{
    [BsonIgnoreExtraElements]
    public class LeadOkVayAddressDto
    {
        [Required]
        public DataConfigDto Province { get; set; }
    }
}
