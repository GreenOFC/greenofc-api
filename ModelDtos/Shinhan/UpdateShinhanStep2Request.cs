using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    public class UpdateShinhanStep2Request
    {
        public ShinhanWorkingDto Working { get; set; }
        public IEnumerable<ShinhanReferenceDto> Referees { get; set; }
    }
}
