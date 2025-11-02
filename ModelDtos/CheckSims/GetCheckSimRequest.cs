using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class GetCheckSimRequest : PagingRequest
    {
        public CheckSimProject Project { get; set; }
        public string Customer { get; set; }
        public TransactionStatus? Status { get; set; }
        public string Pos { get; set; }
    }
}
