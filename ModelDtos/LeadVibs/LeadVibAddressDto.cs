using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadVibs
{
    public class LeadVibAddressDto
    {
        public DataConfigDto Province { get; set; }
        public DataConfigDto District { get; set; }
        public DataConfigDto Ward { get; set; }
        public string Street { get; set; }
    }
}
