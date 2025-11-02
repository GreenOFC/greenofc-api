using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Pos
{
    public class UpdatePosDto
    {
        public string Name { get; set; }

        public string SaleChanelId { get; set; }

        public IEnumerable<string> AddedConcurrentManagerIds { get; set; }

        public IEnumerable<string> RemovedConcurrentManagerIds { get; set; }
    }
}
