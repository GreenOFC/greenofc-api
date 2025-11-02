using Refit;

namespace _24hplusdotnetcore.ModelDtos.MCDebts
{
    public class GetMCDebtRequest : PagingRequest
    {
        public MCDebtStatus? Status { get; set; }
    }

    public enum MCDebtStatus
    {
        Paid = 1,
        UnPaid = 2,
        UnFollow = 3
    }
}
