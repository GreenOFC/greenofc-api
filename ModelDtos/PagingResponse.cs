using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos
{
    public class PagingResponse<T>
    {
        public long TotalRecord { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
