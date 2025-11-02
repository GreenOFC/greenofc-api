namespace _24hplusdotnetcore.ModelDtos.Transaction
{
    public class TransactionIpnRequest: PagingRequest
    {
        public string Transaction { get; set; }
        public string PartnerTransaction { get; set; }
    }
}
