using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniLoanDetailResponse
    {
        public int? Status { get; set; }
        public long? CreateTime { get; set; }
        public long? UpdateTime { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CaseId { get; set; }
        public string LoanId { get; set; }
        public string ApplicationId { get; set; }
        public bool? IsDraft { get; set; }
        public object Description { get; set; }
        public string SaleId { get; set; }
        public int? GroupStatus { get; set; }
        public long? LastCallBackTime { get; set; }
        public PtfOmniLoanDetailRequestInfo RequestInfo { get; set; }
        public PtfOmniLoanDetailSaleInfo SaleInfo { get; set; }
        public PtfOmniLoanDetailCustomerInfo CustomerInfo { get; set; }
        public List<PtfOmniLoanDetailReferenceInfo> ReferenceInfos { get; set; }
        public PtfOmniLoanDetailContractInfo ContractInfo { get; set; }
        public PtfOmniLoanDetailEmploymentInfo EmploymentInfo { get; set; }
        public PtfOmniLoanDetailDisbursementInfo DisbursementInfo { get; set; }
        public IEnumerable<string> ListAssigned { get; set; }
        public string QueueName { get; set; }
        public string PreQueue { get; set; }
        public string Decision { get; set; }
        public string PreDecision { get; set; }
        public string CurrentRemark { get; set; }
        public string LeaderNote { get; set; }
        public string PreRemark { get; set; }
        public object CreditApproval { get; set; }
    }

    public class PtfOmniLoanDetailRequestInfo
    {
        public string QuickProcessEnable { get; set; }
        public string AllowChannel { get; set; }
        public string ProductType { get; set; }
        public string CreditProduct { get; set; }
        public string LoanId { get; set; }
        public string ApplicationDate { get; set; }
        public int? RequestLoanAmount { get; set; }
        public string LoanCurrency { get; set; }
        public string LoanPurpose { get; set; }
        public string Term { get; set; }
        public string InterestRate { get; set; }
        public string DueDate { get; set; }
        public string CampaignId { get; set; }
        public bool? Insurance { get; set; }
        public string InsuranceProductCode { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsuranceFeeRate { get; set; }
        public string InsuredAmountRate { get; set; }
        public string InsuranceXDay { get; set; }
        public string ActualLoanAmount { get; set; }
        public string InsuranceFeeAmount { get; set; }
        public string OtherProduct { get; set; }
    }

    public class PtfOmniLoanDetailSaleInfo
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

    public class PtfOmniLoanDetailIdsCustomer
    {
        public string ExpireDate { get; set; }
        public bool? IdDocument2 { get; set; }
        public string IdDocumentNo { get; set; }
        public string IdIssueCity { get; set; }
        public string IdIssueDate { get; set; }
        public string IdType { get; set; }
    }

    public class PtfOmniLoanDetailCustomerInfo
    {
        public string CustomerType { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string FrbDocumentNo { get; set; }
        public List<PtfOmniLoanDetailIdsCustomer> IdsCustomer { get; set; }
        public IEnumerable<object> CifDetail { get; set; }
        public IEnumerable<object> ExitingLoans { get; set; }
        public IEnumerable<object> RejectLoans { get; set; }
        public IEnumerable<object> BlackList { get; set; }
    }

    public class PtfOmniLoanDetailReferenceInfo
    {
        public string IdReference { get; set; }
        public string RelatedPerson { get; set; }
        public string RelatedFullName { get; set; }
        public string Phone { get; set; }
    }

    public class PtfOmniLoanDetailContractInfo
    {
        public string PrimaryMobile { get; set; }
        public string HomePhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string Email { get; set; }
        public string SocialAccountType { get; set; }
        public string SocialAccountDetails { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentWard { get; set; }
        public string PermanentDistrict { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentStreet { get; set; }
        public bool? PermanentSameCurrent { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentDistrict { get; set; }
        public string CurrentWard { get; set; }
        public string CurrentStreet { get; set; }
        public string CurrentAddress { get; set; }
        public string MaritalStatus { get; set; }
        public string DependentPerson { get; set; }
        public string NumberOfChildrens { get; set; }
        public string Education { get; set; }
    }

    public class PtfOmniLoanDetailEmploymentInfo
    {
        public string EcomomicalStatus { get; set; }
        public string Company { get; set; }
        public string Profession { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string Income { get; set; }
        public string EmployedAtLastWork { get; set; }
        public string MonthlyPayOfOtherLoans { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public string IncomeSource { get; set; }
        public string IncomeType { get; set; }
        public string TaxCode { get; set; }
        public string BusinessLicense { get; set; }
        public string IndustryGroup { get; set; }
        public string Industry { get; set; }
        public string IndustryDetails { get; set; }
    }

    public class PtfOmniLoanDetailDisbursementInfo
    {
        public string BankBranch { get; set; }
        public string BankCity { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BenAccountName { get; set; }
        public string BenAccountNumber { get; set; }
        public string DisbursementMethod { get; set; }
        public string PartnerBranch { get; set; }
        public string PartnerName { get; set; }
    }
}
