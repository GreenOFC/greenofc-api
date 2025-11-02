namespace _24hplusdotnetcore.ModelDtos.eWalletTransaction
{
    public class TransactionRequest: PagingRequest
    {
        public string Status { get; set; }
        public string BillType { get; set; }
        public string BillStatus { get; set; }
        public string Amount { get; set; }
        public string PartnerTransaction { get; set; }
        public string Type { get; set; }
    }
}
