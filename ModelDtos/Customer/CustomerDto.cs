using _24hplusdotnetcore.ModelDtos.GroupDocuments;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace _24hplusdotnetcore.ModelDtos.Customer
{
    [BsonIgnoreExtraElements]
    public class CustomerDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [JsonProperty("mcfcId")]
        public int MAFCId { get; set; }
        [JsonProperty("crmId")]
        public string CRMId { get; set; }
        [JsonProperty("mcId")]
        public int MCId { get; set; }
        [JsonProperty("mcAppnumber")]
        public int MCAppnumber { get; set; }
        [JsonProperty("mcAppId")]
        public string MCAppId { get; set; }
        [JsonProperty("contractCode")]
        public string ContractCode { get; set; } // @todo
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
        [JsonProperty("personal")]
        public PersonalDto Personal { get; set; } // 80%
        [JsonProperty("residentAddress")]
        public AddressDto ResidentAddress { get; set; } // @todo
        [JsonProperty("isTheSameResidentAddress")]
        public Boolean IsTheSameResidentAddress { get; set; }
        [JsonProperty("temporaryAddress")]
        public AddressDto TemporaryAddress { get; set; } // @todo
        [JsonProperty("working")]
        public WorkingDto Working { get; set; }
        [JsonProperty("spouse")]
        public RefereeDto Spouse { get; set; }
        [JsonProperty("referees")]
        public IEnumerable<RefereeDto> Referees { get; set; }
        [JsonProperty("loan")]
        public LoanDto Loan { get; set; }
        [JsonProperty("bankInfo")]
        public BankInfoDto BankInfo { get; set; }
        [JsonProperty("saleInfo")]
        public SaleDto SaleInfo { get; set; }
        [JsonProperty("otherInfo")]
        public OtherInfoDto OtherInfo { get; set; }
        [JsonProperty("result")]
        public ResultDto Result { get; set; }
        [JsonProperty("recordFile")]
        public UploadedMediaDto RecordFile { get; set; }
        [JsonProperty("recordFileBackup")]
        public UploadedMediaDto RecordFileBackup { get; set; }
        [JsonProperty("documents")]
        public IEnumerable<GroupDocumentDto> Documents { get; set; }
        [JsonProperty("returnDocuments")]
        public IEnumerable<GroupDocumentDto> ReturnDocuments { get; set; }
        public DisbursementInformationDto DisbursementInformation { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string Creator { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Modifier { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class PersonalDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } // potentialname
        [JsonProperty("idCard")]
        public string IdCard { get; set; } // cf_1050
        [JsonProperty("oldIdCard")]
        public string OldIdCard { get; set; } // cf_1050
        [JsonProperty("idCardProvince")]
        public string IdCardProvince { get; set; } // 
        [JsonProperty("idCardDate")]
        public string IdCardDate { get; set; } // cf_1350
        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; } // cf_948
        [JsonProperty("phone")]
        public string Phone { get; set; } // cf_854
        [JsonProperty("subPhone")]
        public string SubPhone { get; set; } // cf_854
        [JsonProperty("educationLevel")]
        public string EducationLevel { get; set; } // @todo
        [JsonProperty("maritalStatus")]
        public string MaritalStatus { get; set; } //cf_1030
        [JsonProperty("gender")]
        public string Gender { get; set; } // cf_1026
        [JsonProperty("email")]
        public string Email { get; set; } // cf_1028
        [JsonProperty("noOfDependent")]
        public string NoOfDependent { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class AddressDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("propertyStatus")]
        public string PropertyStatus { get; set; }
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
    }

    [BsonIgnoreExtraElements]
    public class WorkingDto
    {
        [JsonProperty("constitution")]
        public string Constitution { get; set; }
        [JsonProperty("priority")]
        public string Priority { get; set; }
        [JsonProperty("job")]
        public string Job { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("income")]
        public string Income { get; set; }
        [JsonProperty("incomeMethod")]
        public string IncomeMethod { get; set; }
        [JsonProperty("dueDay")]
        public string DueDay { get; set; }
        [JsonProperty("laborType")]
        public string LaborType { get; set; }
        [JsonProperty("otherLoans")]
        public string OtherLoans { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
        [JsonProperty("companyTaxCode")]
        public string CompanyTaxCode { get; set; }
        [JsonProperty("companyTaxSubCode")]
        public string CompanyTaxSubCode { get; set; }
        [JsonProperty("companyPhone")]
        public string CompanyPhone { get; set; }
        [JsonProperty("companyAddress")]
        public AddressDto CompanyAddress { get; set; }
        [JsonProperty("workPeriod")]
        public string WorkPeriod { get; set; }
        [JsonProperty("typeOfContract")]
        public string TypeOfContract { get; set; }
        [JsonProperty("healthCardInssurance")]
        public string HealthCardInssurance { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SaleDto
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
        public string FullName { get; set; }

    }

    [BsonIgnoreExtraElements]
    public class OtherInfoDto
    {
        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }
        [JsonProperty("secretWith")]
        public string SecretWith { get; set; }
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
    public class RefereeDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("idCard")]
        public string IdCard { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("relationship")]
        public string Relationship { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LoanDto
    {
        [JsonProperty("purpose")]
        public string Purpose { get; set; }
        [JsonProperty("term")]
        public string Term { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("buyInsurance")]
        public Boolean BuyInsurance { get; set; } = true;
        [JsonProperty("signAddress")]
        public string SignAddress { get; set; }
        [JsonProperty("paymentDate")]
        public string PaymentDate { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class BankInfoDto
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
    public class ResultDto
    {
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("statusValue")]
        public string StatusValue { get; set; }
        [JsonProperty("detailStatus")]
        public string DetailStatus { get; set; }
        [JsonProperty("detailStatusValue")]
        public string DetailStatusValue { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("returnStatus")]
        public string ReturnStatus { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        public decimal? ApprovedAmount { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class DisbursementInformationDto
    {
        public string DisbursementMethod { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankBranchCode { get; set; }
        public string Province { get; set; }
        public string BankAccount { get; set; }
        public string PartnerName { get; set; }
        public string PartnerBranch { get; set; }
        public string SipCode { get; set; }
    }
}
