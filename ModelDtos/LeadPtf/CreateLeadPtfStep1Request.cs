using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class CreateLeadPtfStep1Request: ICreateLeadPtf
    {
        [Required]
        public string ProductLine { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        public LeadPtfPersonalDto Personal { get; set; }
        public string MobileVersion { get; set; }
    }
}
