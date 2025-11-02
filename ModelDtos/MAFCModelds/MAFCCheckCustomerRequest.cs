using _24hplusdotnetcore.Common.Constants;

namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCCheckCustomerRequest
    {
        public string SearchVal { get; set; }
        public string Partner { get; set; } = MAFCDataEntry.UserId;
    }
    public class MAFCCheckCustomerV3Request
    {
        public string CMND { get; set; }
        public string Phone { get; set; }
        public string TaxCode { get; set; }
        public string CustomerName { get; set; }
        public string SaleCode { get; set; }
        public string Partner { get; set; } = MAFCDataEntry.UserId;
    }
}
