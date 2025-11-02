using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECFullLoanDto
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("partner_code")]
        public string PartnerCode { get; set; }

        [JsonProperty("dsa_agent_code")]
        public string DsaAgentCode { get; set; }

        [JsonProperty("sales_code")]
        public string SalesCode { get; set; }

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("identity_card_id")]
        public string IdentityCardId { get; set; }

        [JsonProperty("issue_place")]
        public string IssuePlace { get; set; }

        [JsonProperty("issue_date")]
        public string IssueDate { get; set; }

        [JsonProperty("identity_card_id_2")]
        public string IdentityCardId2 { get; set; }

        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("employment_type")]
        public string EmployeeType { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("loan_amount")]
        public decimal? LoanAmount { get; set; }

        [JsonProperty("loan_tenor")]
        public int? LoanTenor { get; set; }

        [JsonProperty("tem_province")]
        public string TemProvince { get; set; }

        [JsonProperty("tem_district")]
        public string TemDistrict { get; set; }

        [JsonProperty("tem_ward")]
        public string TemWard { get; set; }

        [JsonProperty("tem_address")]
        public string TemAddress { get; set; }

        [JsonProperty("years_of_stay")]
        public string YearsOfStay { get; set; }

        [JsonProperty("permanent_province")]
        public string PermanentProvince { get; set; }

        [JsonProperty("permanent_district")]
        public string PermanentDistrict { get; set; }

        [JsonProperty("permanent_ward")]
        public string PermanentWard { get; set; }

        [JsonProperty("permanent_address")]
        public string PermanentAddress { get; set; }

        [JsonProperty("profession")]
        public string Profession { get; set; }

        [JsonProperty("married_status")]
        public string MarriedStatus { get; set; }

        [JsonProperty("house_type")]
        public string HouseType { get; set; }

        [JsonProperty("number_of_dependents")]
        public string NumberOfDependents { get; set; }

        [JsonProperty("disbursement_method")]
        public string DisbursementMethod { get; set; }

        [JsonProperty("beneficiary_name")]
        public string BeneficiaryName { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("bank_branch_code")]
        public string BankBranchCode { get; set; }

        [JsonProperty("bank_account")]
        public string BankAccount { get; set; }

        [JsonProperty("monthly_income")]
        public decimal? MonthlyIncome { get; set; }

        [JsonProperty("other_income")]
        public decimal? OtherIncome { get; set; }

        [JsonProperty("monthly_expense")]
        public decimal? MonthlyExpense { get; set; }

        [JsonProperty("Job_type")]
        public string JobType { get; set; }

        [JsonProperty("income_method")]
        public string IncomeMethod { get; set; }

        [JsonProperty("income_frequency")]
        public string IncomeFrequency { get; set; }

        [JsonProperty("income_receiving_date")]
        public string IncomeReceivingDate { get; set; }

        [JsonProperty("job_title")]
        public string JobTitle { get; set; }

        [JsonProperty("workplace_name")]
        public string WorkplaceName { get; set; }

        [JsonProperty("workplace_province")]
        public string WorkplaceProvince { get; set; }

        [JsonProperty("workplace_district")]
        public string WorkplaceDistrict { get; set; }

        [JsonProperty("workplace_ward")]
        public string WorkplaceWard { get; set; }

        [JsonProperty("workplace_address")]
        public string WorkplaceAddress { get; set; }

        [JsonProperty("workplace_phone")]
        public string WorkplacePhone { get; set; }

        [JsonProperty("employment_contract")]
        public string EmploymentContract { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("contract_term")]
        public string ContractTerm { get; set; }

        [JsonProperty("tax_id")]
        public string TaxId { get; set; }

        [JsonProperty("loan_purpose")]
        public string LoanPurpose { get; set; }

        [JsonProperty("other_contact")]
        public string OtherContact { get; set; }

        [JsonProperty("detail_contact")]
        public string DetailContact { get; set; }

        [JsonProperty("relation_1")]
        public string Relation1 { get; set; }

        [JsonProperty("relation_1_name")]
        public string Relation1Name { get; set; }

        [JsonProperty("relation_1_phone_number")]
        public string Relation1PhoneNumber { get; set; }

        [JsonProperty("relation_2")]
        public string Relation2 { get; set; }

        [JsonProperty("relation_2_name")]
        public string Relation2Name { get; set; }

        [JsonProperty("relation_2_phone_number")]
        public string Relation2PhoneNumber { get; set; }

        [JsonProperty("mailing_address")]
        public string MailingAddress { get; set; }

        [JsonProperty("lending_method")]
        public string LendingMethod { get; set; }

        [JsonProperty("business_date")]
        public string BusinessDate { get; set; }

        [JsonProperty("business_license_number")]
        public string BusinessLicenseNumber { get; set; }

        [JsonProperty("annual_revenue")]
        public string AnnualRevenue { get; set; }

        [JsonProperty("annual_profit")]
        public string AnnualProfit { get; set; }

        [JsonProperty("monthly_revenue")]
        public string MonthlyRevenue { get; set; }

        [JsonProperty("monthly_profit")]
        public string MonthlyProfit { get; set; }

        [JsonProperty("doc_collecting_list")]
        public IEnumerable<ECDocCollectingListDto> DocCollectingList { get; set; }
        [JsonProperty("img_id_card")]
        public string ImgIdCard { get; set; }
        [JsonProperty("img_selfie")]
        public string ImgSelfie { get; set; }
    }
    
    public class ECDocCollectingListDto
    {
        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }
    }
}
