using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class UpdateMcLeadStep1Request
    {
        public McLeadPersonalDto Personal { get; set; }
        public IEnumerable<McReferenceDto> Referees { get; set; }
        public McAddressDto TemporaryAddress { get; set; }
    }
}
