using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    [BsonIgnoreExtraElements]
    public class LeadEcDisbursementInformationDto
    {
        public string DisbursementMethodId { get; set; }
        public string DisbursementMethod { get; set; }
        [StringLength(80, MinimumLength = 1)]
        public string BeneficiaryName { get; set; }
        public string BankCodeId { get; set; }
        public string BankCode { get; set; }
        public string BankBranchCodeId { get; set; }
        public string BankBranchCode { get; set; }
        [StringLength(50)]
        public string BankAccount { get; set; }
    }
}
