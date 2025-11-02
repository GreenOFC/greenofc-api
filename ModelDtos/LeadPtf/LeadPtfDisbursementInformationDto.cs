using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfDisbursementInformationDto
    {
        public string DisbursementMethodId { get; set; }
        public string DisbursementMethod { get; set; }
        public string BankCodeId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankBranchCodeId { get; set; }
        public string BankBranchCode { get; set; }
        public string Province { get; set; }
        public string ProvinceId { get; set; }
        public string BankAccount { get; set; }
        public string BeneficiaryName { get; set; }
        public string PartnerName { get; set; }
        public string PartnerNameId { get; set; }
        public string PartnerBranch { get; set; }
        public string SipCode { get; set; }
    }
}
