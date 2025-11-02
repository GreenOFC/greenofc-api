using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    public class CreateShinhanRequest
    {
        public ShinhanPersonalDto Personal { get; set; }
        public ShinhanWorkingDto Working { get; set; }
        public IEnumerable<ShinhanReferenceDto> Referees { get; set; }
        public ShinhanLoanDto Loan { get; set; }
        public ShinhanAddressDto ResidentAddress { get; set; }
        public ShinhanAddressDto TemporaryAddress { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public string ProductLine { get; set; }
        public string MobileVersion { get; set; }
    }
}
