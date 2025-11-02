using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniLoanListResponse
    {
        public long? CreateTime { get; set; }
        public long? UpdateTime { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string LoanId { get; set; }
        public string UserId { get; set; }
        public string CaseId { get; set; }
        public string ApplicationId { get; set; }
        public bool? IsDraft { get; set; }
        public string SaleId { get; set; }
        public string QueueName { get; set; }
        public string Decision { get; set; }
        public PtfOmniLoanListCustomerInfo CustomerInfo { get; set; }
        public PtfOmniLoanListContractInfo ContractInfo { get; set; }
        public PtfOmniLoanListRequestInfo RequestInfo { get; set; }
        public object CreditApproval { get; set; }
        public PtfOmniLoanListSaleInfo SaleInfo { get; set; }
        public long? DateAppSubmit { get; set; }
    }

    public class PtfOmniLoanListIdsCustomer
    {
        public string IdType { get; set; }
        public string IdDocumentNo { get; set; }
        public string IdIssueDate { get; set; }
        public string ExpireDate { get; set; }
        public string IdIssueCity { get; set; }
        public bool? IdDocument2 { get; set; }
    }

    public class PtfOmniLoanListCustomerInfo
    {
        public string CustomerType { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string FrbDocumentNo { get; set; }
        public IEnumerable<PtfOmniLoanListIdsCustomer> IdsCustomer { get; set; }
    }

    public class PtfOmniLoanListContractInfo
    {
        public string PrimaryMobile { get; set; }
        public string Email { get; set; }
    }

    public class PtfOmniLoanListRequestInfo
    {
        public string ProductType { get; set; }
        public string CreditProduct { get; set; }
        public string ApplicationDate { get; set; }
        public int? RequestLoanAmount { get; set; }
        public string LoanPurpose { get; set; }
        public string Term { get; set; }
    }

    public class PtfOmniLoanListSaleInfo
    {
        public string SaleId { get; set; }
        public string SaleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ChannelType { get; set; }
        public string SubChannel { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Pos { get; set; }
        public string TeamName { get; set; }
    }
}
