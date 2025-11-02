using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class CreateMafcStep1WebRequest
    {
        public string OldCustomerId { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        public string ProductLine { get; set; }
        public MafcPersonalDto Personal { get; set; }
        public MafcRefereeDto Spouse { get; set; }
        public IEnumerable<MafcRefereeDto> Referees { get; set; }
    }
}
