using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    [BsonIgnoreExtraElements]
    public class GetLeadEcWorkingResponse
    {
        public string EmploymentStatus { get; set; }
        public string EmploymentStatusId { get; set; }
        public string Job { get; set; }
        public string JobId { get; set; }
        public string CompanyName { get; set; }
        public LeadEcAddressDto CompanyAddress { get; set; }
        public string TaxCode { get; set; }
        public string Income { get; set; }
        public decimal? OtherIncome { get; set; }
        public decimal? MonthlyExpenese { get; set; }
    }
}
