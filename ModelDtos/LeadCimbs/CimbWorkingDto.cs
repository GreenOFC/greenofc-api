using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbWorkingDto
    {
        public string CompanyName { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentStatusId { get; set; }
        public string Income { get; set; }
        public string TaxCode { get; set; }
    }
}
