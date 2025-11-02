using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.PtfOmnis;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class PtfOmniMappingProfile: Profile
    {
        public PtfOmniMappingProfile()
        {
            CreateMap<PtfOmniMasterDataListResponse, PtfOmniMasterData>();
            CreateMap<PtfOmniMasterData, PtfOmniMasterDataResponse>();

            CreateMap<Customer, PtfOmniLoanCreateRequest>()
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.PtfCaseId))
                .ForMember(dest => dest.IsDraft, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ContractInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CustomerInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.DisbursementInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.EmploymentInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.RequestInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.ReferenceInfos, opt => opt.MapFrom(src => src.Referees));

            CreateMap<Customer, PtfOmniLoanUpdateRequest>()
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.PtfCaseId))
                .ForMember(dest => dest.IsDraft, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ContractInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CustomerInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.DisbursementInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.EmploymentInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.RequestInfo, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.ReferenceInfos, opt => opt.MapFrom(src => src.Referees));

            CreateMap<Customer, PtfOmniLoanContractInfo>()
                .ForMember(dest => dest.CurrentCity, opt => opt.MapFrom(src => src.TemporaryAddress.ProvinceId))
                .ForMember(dest => dest.CurrentDistrict, opt => opt.MapFrom(src => src.TemporaryAddress.DistrictId))
                .ForMember(dest => dest.CurrentWard, opt => opt.MapFrom(src => src.TemporaryAddress.WardId))
                .ForMember(dest => dest.CurrentStreet, opt => opt.MapFrom(src => src.TemporaryAddress.Street))
                .ForMember(dest => dest.PermanentSameCurrent, opt => opt.MapFrom(src => src.IsTheSameResidentAddress))
                .ForMember(dest => dest.PermanentCity, opt => opt.MapFrom(src => src.ResidentAddress.ProvinceId))
                .ForMember(dest => dest.PermanentDistrict, opt => opt.MapFrom(src => src.ResidentAddress.DistrictId))
                .ForMember(dest => dest.PermanentWard, opt => opt.MapFrom(src => src.ResidentAddress.WardId))
                .ForMember(dest => dest.PermanentStreet, opt => opt.MapFrom(src => src.ResidentAddress.Street))
                .ForMember(dest => dest.DependentPerson, opt => opt.MapFrom(src => src.Personal.DependentPersonId))
                .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Personal.EducationLevelId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.TemporaryAddress.Email))
                .ForMember(dest => dest.HomePhone, opt => opt.MapFrom(src => src.TemporaryAddress.FixedPhone))
                .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => src.Personal.MaritalStatusId))
                .ForMember(dest => dest.NumberOfChildrens, opt => opt.MapFrom(src => src.Personal.NoOfDependent))
                .ForMember(dest => dest.PrimaryMobile, opt => opt.MapFrom(src => src.Personal.Phone))
                .ForMember(dest => dest.SecondaryPhone, opt => opt.MapFrom(src => src.Personal.OldPhone))
                .ForMember(dest => dest.SocialAccountType, opt => opt.MapFrom(src => src.Working.SocialAccountId))
                .ForMember(dest => dest.SocialAccountDetails, opt => opt.MapFrom(src => src.Working.SocialAccountDetail));

            CreateMap<Customer, PtfOmniLoanCustomerInfo>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerTypeId)) // Hard code: Khach hang moi
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Personal.TitleId))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => $"{src.Personal.GetDateOfBirth():yyyy-MM-dd}"))
                .ForMember(dest => dest.FrbDocumentNo, opt => opt.MapFrom(src => src.FamilyBookNo))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Personal.Name.ToUpper()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Personal.Gender == "Nam" ? "1" : "2"));
                //.ForMember(dest => dest.IdsCustomer, opt => opt.MapFrom(src => src)) // Add by code

            CreateMap<Customer, PtfOmniLoanDisbursementInfo>()
                .ForMember(dest => dest.BankBranch, opt => opt.MapFrom(src => src.DisbursementInformation.BankBranchCodeId))
                .ForMember(dest => dest.BankCity, opt => opt.MapFrom(src => src.DisbursementInformation.ProvinceId))
                .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.DisbursementInformation.BankCodeId))
                .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.DisbursementInformation.BankCodeId))
                .ForMember(dest => dest.BenAccountName, opt => opt.MapFrom(src => src.DisbursementInformation.BeneficiaryName))
                .ForMember(dest => dest.BenAccountNumber, opt => opt.MapFrom(src => src.DisbursementInformation.BankAccount))
                .ForMember(dest => dest.DisbursementMethod, opt => opt.MapFrom(src => src.DisbursementInformation.DisbursementMethodId))
                .ForMember(dest => dest.PartnerBranch, opt => opt.MapFrom(src => src.DisbursementInformation.PartnerBranch))
                .ForMember(dest => dest.PartnerName, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.DisbursementInformation.PartnerNameId) ? src.DisbursementInformation.PartnerName : ""));

            CreateMap<Customer, PtfOmniLoanEmploymentInfo>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Working.CompanyAddress.ProvinceId))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Working.CompanyAddress.DistrictId))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Working.CompanyAddress.WardId))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Working.CompanyAddress.Street))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Working.CompanyName))
                .ForMember(dest => dest.CompanyPhoneNumber, opt => opt.MapFrom(src => src.Working.CompanyPhone.Replace(" ", string.Empty)))
                .ForMember(dest => dest.EcomomicalStatus, opt => opt.MapFrom(src => src.Working.EmploymentStatusId))
                .ForMember(dest => dest.EmployedAtLastWork, opt => opt.MapFrom(src => $"{src.Working.GetDateStartWork():yyyy-MM-dd}"))
                .ForMember(dest => dest.Income, opt => opt.MapFrom(src => src.Working.Income))
                .ForMember(dest => dest.IncomeSource, opt => opt.MapFrom(src => src.Working.IncomeSourceId))
                .ForMember(dest => dest.IncomeType, opt => opt.MapFrom(src => src.Working.IncomeMethodId))
                .ForMember(dest => dest.IndustryGroup, opt => opt.MapFrom(src => src.Working.IndustryGroupId))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Working.IndustryId))
                .ForMember(dest => dest.IndustryDetails, opt => opt.MapFrom(src => src.Working.IndustryDetailsId))
                .ForMember(dest => dest.MonthlyPayOfOtherLoans, opt => opt.MapFrom(src => src.Loan.TotalPaymentsToCreditInstitution.ToString() ?? "0"))
                .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Working.PositionId))
                .ForMember(dest => dest.BusinessLicense, opt => opt.MapFrom(src => src.Working.BusinessLicense))
                .ForMember(dest => dest.TaxCode, opt => opt.MapFrom(src => src.Working.TaxCode));

            CreateMap<Customer, PtfOmniLoanRequestInfo>()
                .ForMember(dest => dest.AllowChannel, opt => opt.MapFrom(src => "3P")) // 3P
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Loan.CategoryId ?? "1")) // 1: cash
                .ForMember(dest => dest.CreditProduct, opt => opt.MapFrom(src => src.Loan.ProductId))
                // .ForMember(dest => dest.CampaignId, opt => opt.MapFrom(src => "")) // TODO: wait for the addition of field in customer
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.Loan.PaymentDate))
                .ForMember(dest => dest.Insurance, opt => opt.MapFrom(src => src.Loan.BuyInsurance))
                .ForMember(dest => dest.InsuranceProductCode, opt => opt.MapFrom(src => src.Loan.BuyInsurance ? src.Loan.InsuranceServiceId : ""))
                .ForMember(dest => dest.LoanPurpose, opt => opt.MapFrom(src => src.Loan.PurposeId))
                .ForMember(dest => dest.QuickProcessEnable, opt => opt.MapFrom(src => "no"))
                .ForMember(dest => dest.RequestLoanAmount, opt => opt.MapFrom(src => src.Loan.GetAmount()))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Loan.TermId))
                .ForMember(dest => dest.OtherProduct, opt => opt.MapFrom(src => src.Loan.Note))
                .ForMember(dest => dest.LoanCurrency, opt => opt.MapFrom(src => "15")) // Vietnamdong
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.Loan.InterestType == "RANK" ? src.Loan.InterestRate : "")); // TODO: wait for the addition of field in customer

            CreateMap<Referee, PtfOmniLoanReferenceInfo>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.RelatedFullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RelatedPerson, opt => opt.MapFrom(src => src.RelationshipId));

        }
    }
}
