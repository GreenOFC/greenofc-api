using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace _24hplusdotnetcore.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection(MongoCollection.CustomerCollection)]
    public class Customer : BaseEntity
    {
        [JsonProperty("mcfcId")]
        public int MAFCId { get; set; }
        [JsonProperty("crmId")]
        public string CRMId { get; set; }
        [JsonProperty("mcId")]
        public int MCId { get; set; }
        [JsonProperty("ec_request")]
        public string ECRequest { get; set; }
        [JsonProperty("mcAppnumber")]
        public int MCAppnumber { get; set; }
        [JsonProperty("mcAppId")]
        public string MCAppId { get; set; }
        [JsonProperty("contractCode")]
        public string ContractCode { get; set; } // @todo
        public string ContractId { get; set; }
        public string CimbId { get; set; }
        public string PtfCaseId { get; set; }
        public DateTime TimeToRunJob { get; set; }
        public bool IsCheckOnboardCimb { get; set; }
        [BsonRequired]
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [BsonRequired]
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("mobileVersion")]
        public string MobileVersion { get; set; }
        [JsonProperty("greenType")]
        public string GreenType { get; set; } // @todo
        [JsonProperty("productLine")]
        public string ProductLine { get; set; }
        [JsonProperty("familyBookNo")]
        public string FamilyBookNo { get; set; }
        [JsonProperty("caseNote")]
        public string CaseNote { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        [JsonProperty("personal")]
        public Personal Personal { get; set; } // 80%
        [JsonProperty("residentAddress")]
        public Address ResidentAddress { get; set; } // @todo
        [JsonProperty("isTheSameResidentAddress")]
        public Boolean IsTheSameResidentAddress { get; set; }
        [JsonProperty("temporaryAddress")]
        public Address TemporaryAddress { get; set; } // @todo
        [JsonProperty("working")]
        public Working Working { get; set; }
        [JsonProperty("spouse")]
        public Referee Spouse { get; set; }
        [JsonProperty("referees")]
        public IEnumerable<Referee> Referees { get; set; }
        [JsonProperty("loan")]
        public Loan Loan { get; set; }
        [JsonProperty("bankInfo")]
        public BankInfo BankInfo { get; set; }
        [JsonProperty("saleInfo")]
        public Sale SaleInfo { get; set; }
        [JsonProperty("otherInfo")]
        public OtherInfo OtherInfo { get; set; }
        [JsonProperty("result")]
        public Result Result { get; set; } = new Result();
        public OldCustomer OldCustomer { get; set; }
        [JsonProperty("recordFile")]
        public UploadedMedia RecordFile { get; set; }
        [JsonProperty("recordFileBackup")]
        public UploadedMedia RecordFileBackup { get; set; }
        [JsonProperty("documents")]
        public IEnumerable<GroupDocument> Documents { get; set; }
        [JsonProperty("returnDocuments")]
        public IEnumerable<GroupDocument> ReturnDocuments { get; set; }
        public DisbursementInformation DisbursementInformation { get; set; }
        public LeadEcSelectedOffer LeadEcSelectedOffer { get; set; }

        public TeamLeadInfo TeamLeadInfo { get; set; }
        public TeamLeadInfo AsmInfo { get; set; }

        public PosInfo PosInfo { get; set; }

        public SaleChanelInfo SaleChanelInfo { get; set; }

        public string GetDocumentType()
        {
            if (Documents == null)
            {
                return null;
            }

            var documentTypes = new List<string>();
            foreach (var group in Documents?.Where(x => x.Documents != null))
            {
                foreach (var doc in group.Documents.Where(y => !string.IsNullOrEmpty(y.DocumentName)))
                {
                    documentTypes.Add(doc.DocumentName.ToUpper());
                }
            }
            string documentType = string.Join(" |##| ", documentTypes);
            return documentType;
        }
        public string GetDocumentUri()
        {
            if (Documents == null)
            {
                return null;
            }

            var documentUris = new List<string>();
            foreach (var group in Documents.Where(x => x.Documents != null))
            {
                foreach (var doc in group.Documents.Where(y => y.UploadedMedias != null))
                {
                    foreach (var media in doc.UploadedMedias.Where(z => !string.IsNullOrEmpty(z.Uri)))
                    {
                        documentUris.Add(media.Uri);
                    }
                }
            }
            string documentUri = string.Join("; ", documentUris);
            return documentUri;
        }

        public string GetReturnDocumentUri()
        {
            if (ReturnDocuments == null)
            {
                return null;
            }

            var documentUris = new List<string>();
            foreach (var group in ReturnDocuments.Where(x => x.Documents != null))
            {
                foreach (var doc in group.Documents.Where(y => y.UploadedMedias != null))
                {
                    foreach (var media in doc.UploadedMedias.Where(z => !string.IsNullOrEmpty(z.Uri)))
                    {
                        documentUris.Add(media.Uri);
                    }
                }
            }
            string documentUri = string.Join("; ", documentUris);
            return documentUri;
        }

        public string GetLeadSource()
        {
            string result = "";
            switch (GreenType)
            {
                case "A":
                    result = "MobileGreenA";
                    break;
                case "C":
                    if (ProductLine == ProductLineConst.Lead)
                    {
                        result = "MC-Lead";
                    }
                    else if (ProductLine == "TSA")
                    {
                        result = "MobileGreenC";
                    }
                    else
                    {
                        result = "MobileGreenC-DSA";
                    }
                    break;
                case "E":
                    result = "MobileGreenE";
                    break;
                case "G":
                    result = "MobileGreenG";
                    break;
                case "D":
                    result = "MobileGreenD";
                    break;
                case Common.GreenType.GreenP:
                    result = "PTF";
                    break;
                case Common.GreenType.GreenF88:
                    result = "MobileGreenF";
                    break;
                default:
                    break;
            }

            return result;
        }

        public LeadSourceType? GetLeadSourceType()
        {
            if (GreenTypeLeadSourceMapping.GREEN_TYPE_LEAD_SOURCE.TryGetValue(GreenType, out LeadSourceType leadSourceType))
            {
                return leadSourceType;
            }
            return null;
        }
        public string GetFinanceCompany()
        {
            string result = "";
            switch (GreenType)
            {
                case Common.GreenType.GreenA:
                    result = "MIRAE ASSET";
                    break;
                case Common.GreenType.GreenC:
                    result = "MCREDIT";
                    break;
                case Common.GreenType.GreenE:
                    result = "SHINHAN";
                    break;
                case Common.GreenType.GreenG:
                    result = "CIMB";
                    break;
                case Common.GreenType.GreenD:
                    result = "EASY CREDIT";
                    break;
                case Common.GreenType.GreenP:
                    result = "PTF";
                    break;
                case Common.GreenType.GreenF88:
                    result = "F88";
                    break;
                default:
                    break;
            }

            return result;
        }

        public void UpdateMandatoryDocument()
        {
            var poaGroup = Documents?.FirstOrDefault(x => x.GroupId == LeadPtfCheckListGroupId.Poa);
            if (poaGroup == null)
            {
                return;
            }

            poaGroup.Mandatory = !IsTheSameResidentAddress;
        }
    }

    [BsonIgnoreExtraElements]
    public class Personal
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        public string TitleId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } // potentialname
        [JsonProperty("idCard")]
        public string IdCard { get; set; } // cf_1050
        [JsonProperty("oldIdCard")]
        public string OldIdCard { get; set; } // cf_1050
        public string OldIdCardProvince { get; set; }
        public string OldIdCardProvinceId { get; set; }
        public string OldIdCardDate { get; set; }
        public string OldIdCardExpiredDate { get; set; }

        [JsonProperty("idCardProvince")]
        public string IdCardProvince { get; set; } // 
        public string IdCardProvinceId { get; set; }
        [JsonProperty("idCardDate")]
        public string IdCardDate { get; set; } // cf_1350
        public string IdCardExpiredDate { get; set; }
        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; } // cf_948
        [JsonProperty("phone")]
        public string Phone { get; set; } // cf_854
        public string OldPhone { get; set; }
        [JsonProperty("subPhone")]
        public string SubPhone { get; set; } // cf_854
        [JsonProperty("educationLevel")]
        public string EducationLevel { get; set; } // @todo
        public string EducationLevelId { get; set; }
        public string DependentPerson { get; set; }
        public string DependentPersonId { get; set; }
        [JsonProperty("maritalStatus")]
        public string MaritalStatus { get; set; } //cf_1030
        public string MaritalStatusId { get; set; }
        public string OtherMaritalStatus { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; } // cf_1026
        [JsonProperty("email")]
        public string Email { get; set; } // cf_1028
        [JsonProperty("noOfDependent")]
        public string NoOfDependent { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public IEnumerable<KeyValueModel> DependentTypes { get; set; }

        public DateTime? GetIdCardDate()
        {
            if (DateTime.TryParseExact(IdCardDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
        public DateTime? GetIdCardExpiredDate()
        {
            if (DateTime.TryParseExact(IdCardExpiredDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
        public DateTime? GetOldIdCardDate()
        {
            if (DateTime.TryParseExact(OldIdCardDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
        public DateTime? GetOldIdCardExpiredDate()
        {
            if (DateTime.TryParseExact(OldIdCardExpiredDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
        public DateTime? GetDateOfBirth()
        {
            if (DateTime.TryParseExact(DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
        public string GetFamilyName()
        {
            return Name.Trim().Split(" ").First();
        }
        public string GetMiddleName()
        {
            var names = Name.Trim().Split(" ");
            if (names.Length > 2)
            {
                var middlesNames = names.Skip(1).SkipLast(1).ToArray();
                return String.Join(" ", middlesNames);
            }
            return "";
        }
        public string GetName()
        {
            return Name.Trim().Split(" ").Last();
        }
        public string GetMAFCMaritalStatus()
        {
            string result = "";
            MAFCDataMapping.PERSONAL_MARIAL_STATUS.TryGetValue(MaritalStatus, out result);
            return result;
        }
        public string GetMAFCEducation()
        {
            string result = "";
            MAFCDataMapping.PERSONAL_EDUCATION.TryGetValue(EducationLevel, out result);
            return result;
        }
        public string GetDependentType()
        {
            return string.Join(", ", DependentTypes?.Select(x => x.Value) ?? new List<string>());
        }
    }

    [BsonIgnoreExtraElements]
    public class Address
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("propertyStatus")]
        public string PropertyStatus { get; set; }
        public string PropertyStatusId { get; set; }
        [JsonProperty("province")]
        public string Province { get; set; }
        [JsonProperty("provinceId")]
        public string ProvinceId { get; set; }
        [JsonProperty("district")]
        public string District { get; set; }
        [JsonProperty("districtId")]
        public string DistrictId { get; set; }
        [JsonProperty("ward")]
        public string Ward { get; set; }
        [JsonProperty("wardId")]
        public string WardId { get; set; }
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("fullAddress")]
        public string FullAddress { get; set; }
        [JsonProperty("mailAddress")]
        public string MailAddress { get; set; }
        [JsonProperty("roomNo")]
        public string RoomNo { get; set; }
        [JsonProperty("landLordName")]
        public string LandLordName { get; set; }
        [JsonProperty("landMark")]
        public string LandMark { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("fixedPhone")]
        public string FixedPhone { get; set; }
        [JsonProperty("durationMonth")]
        public int DurationMonth { get; set; } = 0;
        [JsonProperty("durationYear")]
        public int DurationYear { get; set; } = 0;

        public string GetFullAddress()
        {
            return $"{Street}, {Ward}, {District}, {Province}";
        }

        public string GetMAFCPropertyStatus()
        {
            string result = "";
            MAFCDataMapping.ADDRESS_STATUS.TryGetValue(PropertyStatus, out result);
            return result;
        }

        public string GetAddress()
        {
            return string.Join(", ", new List<string> { Street, Ward, District }
                .Where(x => !string.IsNullOrEmpty(x)));
        }
    }

    [BsonIgnoreExtraElements]
    public class Working
    {
        [JsonProperty("constitution")]
        public string Constitution { get; set; }
        public string ConstitutionId { get; set; }
        [JsonProperty("priority")]
        public string Priority { get; set; }
        public string PriorityId { get; set; }
        [JsonProperty("job")]
        public string Job { get; set; }
        public string JobId { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        public string PositionId { get; set; }
        [JsonProperty("income")]
        public string Income { get; set; }
        [JsonProperty("incomeMethod")]
        public string IncomeMethod { get; set; }
        public string IncomeMethodId { get; set; }
        public string IncomeSource { get; set; }
        public string IncomeSourceId { get; set; }
        public string IndustryGroup { get; set; }
        public string IndustryGroupId { get; set; }
        public string Industry { get; set; }
        public string IndustryId { get; set; }
        public string IndustryDetails { get; set; }
        public string IndustryDetailsId { get; set; }
        [JsonProperty("dueDay")]
        public string DueDay { get; set; }
        [JsonProperty("laborType")]
        public string LaborType { get; set; }
        [JsonProperty("otherLoans")]
        public string OtherLoans { get; set; }
        public string TaxCode { get; set; }
        public string BusinessLicense { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
        [JsonProperty("companyTaxCode")]
        public string CompanyTaxCode { get; set; }
        [JsonProperty("companyTaxSubCode")]
        public string CompanyTaxSubCode { get; set; }
        [JsonProperty("companyPhone")]
        public string CompanyPhone { get; set; }
        [JsonProperty("companyAddress")]
        public Address CompanyAddress { get; set; }
        public Address BranchOfficeAddress { get; set; }

        [JsonProperty("workPeriod")]
        public string WorkPeriod { get; set; }
        [JsonProperty("typeOfContract")]
        public string TypeOfContract { get; set; }
        [JsonProperty("healthCardInssurance")]
        public string HealthCardInssurance { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentStatusId { get; set; }

        [JsonProperty("workingStatus")]
        public string WorkingStatus { get; set; }

        public decimal? OtherIncome { get; set; }
        public decimal? MonthlyExpenese { get; set; }
        public string DateStartWork { get; set; }
        public string SocialAccount { get; set; }
        public string SocialAccountId { get; set; }
        public string SocialAccountDetail { get; set; }

        public decimal? GetIncome()
        {
            if (decimal.TryParse(Income, NumberStyles.Number, new CultureInfo("en-US"), out decimal income))
            {
                return income;
            }
            return null;
        }
        public string GetMAFCPriority()
        {
            MAFCDataMapping.WORKING_PRIORITY.TryGetValue($"{Priority}", out string result);
            return result;
        }
        public int GetMAFCConsti()
        {
            MAFCDataMapping.WORKING_CONSTI.TryGetValue($"{Constitution}", out int result);
            return result;
        }
        public string GetMAFCIncomeMethod()
        {
            if (string.IsNullOrEmpty(IncomeMethod))
            {
                return "";
            }
            string result = "";
            MAFCDataMapping.WORKING_INCOME_METHOD.TryGetValue(IncomeMethod, out result);
            return result;
        }
        public DateTime? GetDateStartWork()
        {
            if (DateTime.TryParseExact(DateStartWork, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
    }

    [BsonIgnoreExtraElements]
    public class Sale
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("mafcId")]
        public string MAFCId { get; set; }
        [JsonProperty("mafcCode")]
        public string MAFCCode { get; set; }
        [JsonProperty("mafcName")]
        public string MAFCName { get; set; }
        public string EcDsaCode { get; set; }
        public string EcSaleCode { get; set; }
        public bool ApprovedByAdmin { get; set; }
        public bool ApprovedByASM { get; set; }
        public string FullName { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class OtherInfo
    {
        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }
        [JsonProperty("secretWith")]
        public string SecretWith { get; set; }
        public string SecretWithId { get; set; }
        [JsonProperty("secretWithOther")]
        public string SecretWithOther { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("isDeferData")]
        public bool IsDeferData { get; set; }
        [JsonProperty("isDeferOnlyImage")]
        public bool IsDeferOnlyImage { get; set; }

    }

    [BsonIgnoreExtraElements]
    public class Referee
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        public string TitleId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("idCard")]
        public string IdCard { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("relationship")]
        public string Relationship { get; set; }
        public string RelationshipId { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Loan
    {
        [JsonProperty("purpose")]
        public string Purpose { get; set; }
        [JsonProperty("purposeOther")]
        public string PurposeOther { get; set; }
        public string PurposeId { get; set; }
        [JsonProperty("term")]
        public string Term { get; set; }
        public string TermId { get; set; }
        public string InterestRatePerYear { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        public string ProductType { get; set; }
        public string ProductTypeId { get; set; }
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        public string MinTerm { get; set; }
        public string MaxTerm { get; set; }
        public string InterestType { get; set; }
        public string InterestRate { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("buyInsurance")]
        public Boolean BuyInsurance { get; set; } = true;
        [JsonProperty("signAddress")]
        public string SignAddress { get; set; }
        public string SignAddressId { get; set; }
        [JsonProperty("paymentDate")]
        public string PaymentDate { get; set; }
        public string PaymentDateId { get; set; }
        public string InsuranceService { get; set; }
        public string InsuranceServiceId { get; set; }
        public bool HadInsurance { get; set; }
        public bool HadInsuranceInThePast { get; set; }
        public decimal TotalPaymentsToCreditInstitution { get; set; }
        public int NumberOfCreditInstitutionsInDebt { get; set; }
        public string Note { get; set; }
        public string TimeToBeAbleToAnswerThePhone { get; set; }

        public decimal? GetAmount()
        {
            if (decimal.TryParse(Amount, NumberStyles.Number, new CultureInfo("en-US"), out decimal amount))
            {
                return amount;
            }
            return null;
        }
        public decimal? GetTotalAmount()
        {
            var amount = GetAmount();
            if (amount == null)
            {
                return null;
            }
            if (BuyInsurance)
            {
                return amount * (decimal)1.055;
            }
            return amount;
        }

        public int? GetTerm()
        {
            var match = Regex.Match(Term, "^(\\d+).*$");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int term))
            {
                return term;
            }

            return null;
        }

        public string GetMAFCPurpose()
        {
            string result = "";
            MAFCDataMapping.LOAN_PURPOSE.TryGetValue(this.Purpose, out result);
            return result;
        }

        public DateTime? GetPaymentDate()
        {
            if (DateTime.TryParseExact(PaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
    }

    [BsonIgnoreExtraElements]
    public class BankInfo
    {
        [JsonProperty("isBankAccount")]
        public bool IsBankAccount { get; set; } = true;
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("branch")]
        public string Branch { get; set; }
        [JsonProperty("accountType")]
        public string AccountType { get; set; }
        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Result
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("returnStatus")]
        public string ReturnStatus { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        public bool GeneratePdf { get; set; } = false;
        public bool FinishedRound1 { get; set; } = false;
        public decimal? ApprovedAmount { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ContractNumber { get; set; }
        public string RejectReason { get; set; }
        public string MAFCEcontract { get; set; }
        public string GetRejectCode()
        {
            string result = "";
            if (ReturnStatus == "REJ" && Reason != null)
            {
                result = Reason.Split(" - ")[0];
            }
            return result;
        }
    }

    [BsonIgnoreExtraElements]
    public class DisbursementInformation
    {
        public string DisbursementMethodId { get; set; }
        public string DisbursementMethod { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankCodeId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankBranchCodeId { get; set; }
        public string BankBranchCode { get; set; }
        public string Province { get; set; }
        public string ProvinceId { get; set; }
        public string BankAccount { get; set; }
        public string PartnerName { get; set; }
        public string PartnerNameId { get; set; }
        public string PartnerBranch { get; set; }
        public string SipCode { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LeadEcSelectedOffer
    {
        public string SelectedOfferId { get; set; }

        public string SelectedOfferAmount { get; set; }

        public string SelectedOfferInsuranceType { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class KeyValueModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class OldCustomer
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int MafcId { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
    }
}
