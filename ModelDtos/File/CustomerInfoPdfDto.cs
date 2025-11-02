using _24hplusdotnetcore.Common.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _24hplusdotnetcore.ModelDtos.File
{
    public class CustomerInfoPdfDto
    {
        public Boolean IsTheSameResidentAddress { get; set; }
        public string FamilyBookNo { get; set; }
        public SaleInfoPdfDto SaleInfo { get; set; }
        public PersonalInfoPdfDto Personal { get; set; }
        public AddressInfoPdfDto TemporaryAddress { get; set; }
        public AddressInfoPdfDto ResidentAddress { get; set; } // @todo
        public WorkingInfoPdfDto Working { get; set; }
        public LoanInfoPdfDto Loan { get; set; }
        public RefereeInfoPdfDto Spouse { get; set; }
        public IEnumerable<RefereeInfoPdfDto> Referees { get; set; }
        public BankInfoPdfDto BankInfo { get; set; }
        public OtherInfoPdfDto OtherInfo { get; set; }
    }

    public class SaleInfoPdfDto
    {
        public string Name { get; set; }
        public string MAFCId { get; set; }
        public string MAFCCode { get; set; }
        public string MAFCName { get; set; }
    }

    public class PersonalInfoPdfDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string IdCard { get; set; }
        public string IdCardDate { get; set; }
        public string IdCardProvince { get; set; }
        public string OldIdCard { get; set; }
        public string MaritalStatus { get; set; }
        public string OtherMaritalStatus { get; set; }
        public string EducationLevel { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string NoOfDependent { get; set; }

        public DateTime? GetDateOfBirth()
        {
            if (DateTime.TryParseExact(DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return null;
        }

        public DateTime? GetIdCardDate()
        {
            if (DateTime.TryParseExact(IdCardDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return null;
        }
    }

    public class AddressInfoPdfDto
    {
        public string Type { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public int? DurationYear { get; set; }
        public int? DurationMonth { get; set; }
        public string FixedPhone { get; set; }
        public string PropertyStatus { get; set; }
        public string RoomNo { get; set; }
        public string LandLordName { get; set; }
        public string LandMark { get; set; }
        public string Email { get; set; }

        public string GetFullAddress()
        {
            var listOfAddress = new string[] { Street, Ward, District, Province }
                .Where(x => !string.IsNullOrEmpty(x));

            if (!listOfAddress.Any())
            {
                return string.Empty;
            }
            return string.Join(", ", listOfAddress);
        }

        public string GetMAFCPropertyStatus()
        {
            if (MAFCDataMapping.ADDRESS_STATUS.TryGetValue($"{PropertyStatus}", out string result))
            {
                return result;
            }
            return string.Empty;
        }


    }

    public class WorkingInfoPdfDto
    {
        public string Constitution { get; set; }
        public string Priority { get; set; }
        public string Job { get; set; }
        public string Position { get; set; }
        public string Income { get; set; }
        public string IncomeMethod { get; set; }
        public string DueDay { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyTaxCode { get; set; }
        public string CompanyTaxSubCode { get; set; }
        public AddressInfoPdfDto CompanyAddress { get; set; }
        public string GetMAFCPriority()
        {
            string result = "";
            MAFCDataMapping.WORKING_PRIORITY.TryGetValue(Priority, out result);
            return result;
        }
        public int GetMAFCConsti()
        {
            if(!string.IsNullOrEmpty(Constitution) && MAFCDataMapping.WORKING_CONSTI.TryGetValue(Constitution, out int result))
            {
                return result;
            }

            return 0;
        }
    }

    public class LoanInfoPdfDto
    {
        public string Purpose { get; set; }
        public string PurposeOther { get; set; }
        public string Term { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public string Amount { get; set; }
        public string PaymentDate { get; set; }
        public Boolean BuyInsurance { get; set; }
        public string GetMAFCPurpose()
        {
            string result = "";
            MAFCDataMapping.LOAN_PURPOSE.TryGetValue(this.Purpose, out result);
            return result;
        }
    }
    public class RefereeInfoPdfDto
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }
    }

    public class BankInfoPdfDto
    {
        public bool IsBankAccount { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string AccountType { get; set; }
        public string AccountNo { get; set; }
    }
    public class OtherInfoPdfDto
    {
        public bool IsPublic { get; set; }
        public string SecretWith { get; set; }
        public string SecretWithOther { get; set; }
        public string Note { get; set; }
    }
}
