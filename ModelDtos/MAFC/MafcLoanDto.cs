using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    [BsonIgnoreExtraElements]
    public class MafcLoanDto
    {
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public bool BuyInsurance { get; set; }
        public string Amount { get; set; }
        public string Term { get; set; }
        public string TermId { get; set; }
        public string Purpose { get; set; }
        public string PurposeId { get; set; }
        public string PurposeOther { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentDateId { get; set; }
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        public string MinTerm { get; set; }
        public string MaxTerm { get; set; }
    }
}
