using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniLoanCreateRequest
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

    public class PtfOmniLoanContractInfo
    {
        public string CurrentCity { get; set; }
        public string CurrentDistrict { get; set; }
        public string CurrentStreet { get; set; }
        public string CurrentWard { get; set; }
        public string DependentPerson { get; set; }
        public string Education { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string MaritalStatus { get; set; }
        public string NumberOfChildrens { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentDistrict { get; set; }
        public bool? PermanentSameCurrent { get; set; }
        public string PermanentStreet { get; set; }
        public string PermanentWard { get; set; }
        public string PrimaryMobile { get; set; }
        public string SecondaryPhone { get; set; }
        public string SocialAccountDetails { get; set; }
        public string SocialAccountType { get; set; }
    }

    public class PtfOmniLoanIdsCustomer
    {
        public string ExpireDate { get; set; }
        public bool? IdDocument2 { get; set; }
        public string IdDocumentNo { get; set; }
        public string IdIssueCity { get; set; }
        public string IdIssueDate { get; set; }
        public string IdType { get; set; }
    }

    public class PtfOmniLoanCustomerInfo
    {
        public string CustomerType { get; set; }
        public string Title { get; set; }
        public string Dob { get; set; }
        public string FrbDocumentNo { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public IEnumerable<PtfOmniLoanIdsCustomer> IdsCustomer { get; set; }
    }

    public class PtfOmniLoanDisbursementInfo
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

    public class PtfOmniLoanEmploymentInfo
    {
        public string BusinessLicense { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Company { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string EcomomicalStatus { get; set; }
        public string EmployedAtLastWork { get; set; }
        public string Income { get; set; }
        public string IncomeSource { get; set; }
        public string IncomeType { get; set; }
        public string Industry { get; set; }
        public string IndustryDetails { get; set; }
        public string IndustryGroup { get; set; }
        public string MonthlyPayOfOtherLoans { get; set; }
        public string Profession { get; set; }
        public string Street { get; set; }
        public string TaxCode { get; set; }
        public string Ward { get; set; }
    }

    public class PtfOmniLoanReferenceInfo
    {
        public string Phone { get; set; }
        public string RelatedFullName { get; set; }
        public string RelatedPerson { get; set; }
    }

    public class PtfOmniLoanRequestInfo
    {
        public string AllowChannel { get; set; }
        public string CampaignId { get; set; }
        public string CreditProduct { get; set; }
        public string DueDate { get; set; }
        public bool? Insurance { get; set; }
        public string InsuranceProductCode { get; set; }
        public string LoanPurpose { get; set; }
        public string OtherProduct { get; set; }
        public string ProductType { get; set; }
        public string QuickProcessEnable { get; set; }
        public int? RequestLoanAmount { get; set; }
        public string Term { get; set; }
        public string LoanCurrency { get; set; }
        public string InterestRate { get; set; }
    }
}
