using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class HistoryCallApiLogRequest : PagingRequest
    {
        public string Action { get; set; }
        public string GreenType { get; set; }
    }
}
