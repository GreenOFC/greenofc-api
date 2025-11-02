using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class UpdateMcStep2Request
    {
        public McWorkingDto Working { get; set; }
        public IEnumerable<McReferenceDto> Referees { get; set; }
    }
}
