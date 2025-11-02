using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class UpdateMcRequest
    {
        public McPersonalDto Personal { get; set; }
        public McWorkingDto Working { get; set; }
        public IEnumerable<McReferenceDto> Referees { get; set; }
        public McLoanDto Loan { get; set; }
        public McAddressDto ResidentAddress { get; set; }
        public McAddressDto TemporaryAddress { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public string ProductLine { get; set; }
    }
}
