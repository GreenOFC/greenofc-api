
namespace _24hplusdotnetcore.ModelDtos.eWalletTransaction
{
    public class IpnPaymeRequest
    {
        public string Transaction { get; set; }
        public string PartnerTransaction { get; set; }
        public string PaymentId { get; set; }
        public double MerchantId { get; set; }
        public double? StoreId { get; set; }
        public string PayMethod { get; set; }
        public string PayCode { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public double Total { get; set; }
        public string State { get; set; }
        public string Reason { get; set; }
        public string Desc { get; set; }
        public string ExtraData { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}