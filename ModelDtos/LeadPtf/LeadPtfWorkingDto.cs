using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfWorkingDto
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string PositionId { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentStatusId { get; set; }
        public LeadPtfShortAddressDto CompanyAddress { get; set; }
        public string TaxCode { get; set; }
        public string BusinessLicense { get; set; }
        public string Job { get; set; }
        public string JobId { get; set; }
        [Phone]
        public string CompanyPhone { get; set; }
        public string DateStartWork { get; set; }
        public string SocialAccount { get; set; }
        public string SocialAccountId { get; set; }
        public string SocialAccountDetail { get; set; }
        public string Income { get; set; }
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
    }
}
