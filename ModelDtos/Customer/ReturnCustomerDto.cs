using Refit;

namespace _24hplusdotnetcore.ModelDtos.Customer
{
    public class ReturnCustomerDto
    {
        [AliasAs("customerId")]
        public string CustomerId { get; set; }
        [AliasAs("reason")]
        public string Reason { get; set; }
        [AliasAs("returnStatus")]
        public string ReturnStatus { get; set; }
        [AliasAs("leadsource")]
        public string LeadSource { get; set; }
    }
}
