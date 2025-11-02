using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniLoanUpdateRequest
    {
        public string CaseId { get; set; }
        public bool? IsDraft { get; set; }
        public string SaleId { get; set; }
        public PtfOmniLoanContractInfo ContractInfo { get; set; }
        public PtfOmniLoanCustomerInfo CustomerInfo { get; set; }
        public PtfOmniLoanDisbursementInfo DisbursementInfo { get; set; }
        public PtfOmniLoanEmploymentInfo EmploymentInfo { get; set; }
        public List<PtfOmniLoanReferenceInfo> ReferenceInfos { get; set; }
        public PtfOmniLoanRequestInfo RequestInfo { get; set; }
    }
}
