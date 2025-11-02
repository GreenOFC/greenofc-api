using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    public class UpdateLeadPtfStep4Request: IUpdateLeadPtf
    {
        [MaxLength(20)]
        public string FamilyBookNo { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public LeadPtfAddressDto TemporaryAddress { get; set; }
        public LeadPtfShortAddressDto ResidentAddress { get; set; }
    }
}
