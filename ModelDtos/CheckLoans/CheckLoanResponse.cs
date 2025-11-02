using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.F88;

namespace _24hplusdotnetcore.ModelDtos.CheckLoans
{
    public class CheckLoanResponse
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string ContractCode { get; set; }
        public string Status { get; set; }
        public string ReturnReason { get; set; }
        public string TeamLeadNote { get; set; }
        public string ResultStatus { get; set; }
        public F88PostBackResponse PostBack { get; set; }
    }
}
