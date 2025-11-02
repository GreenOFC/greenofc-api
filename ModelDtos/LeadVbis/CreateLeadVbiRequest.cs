using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadVbis
{
    public class CreateLeadVbiRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string IdCard { get; set; }

        [Required]
        public string Phone { get; set; }

        public string ExtraPhone { get; set; }

        [Required]
        public LeadVbiAddressDto TemporaryAddress { get; set; }
    }
}
