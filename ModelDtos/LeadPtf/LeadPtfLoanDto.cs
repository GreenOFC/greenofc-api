using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{
    [BsonIgnoreExtraElements]
    public class LeadPtfLoanDto
    {
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        public string MinTerm { get; set; }
        public string MaxTerm { get; set; }
        public string InterestType { get; set; }
        public string InterestRate { get; set; }
        public string Amount { get; set; }
        public string Purpose { get; set; }
        public string PurposeId { get; set; }
        public string Term { get; set; }
        public string TermId { get; set; }
        public bool BuyInsurance { get; set; }
        public string InsuranceService { get; set; }
        public string InsuranceServiceId { get; set; }
        public bool HadInsurance { get; set; }
        public bool HadInsuranceInThePast { get; set; }
        public decimal TotalPaymentsToCreditInstitution { get; set; }
        public int NumberOfCreditInstitutionsInDebt { get; set; }
        public string Note { get; set; }
        public string TimeToBeAbleToAnswerThePhone { get; set; }
        public string PaymentDate { get; set; }
    }
}
