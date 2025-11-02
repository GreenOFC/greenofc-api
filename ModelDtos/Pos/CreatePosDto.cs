using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Pos
{
    public class CreatePosDto
    {
        public string Name { get; set; }

        public string SaleChanelId { get; set; }

        public IEnumerable<string> AddedConcurrentManagerIds { get; set; }
    }
}
