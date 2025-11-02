using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.ModelDtos.LeadEcs
{
    [BsonIgnoreExtraElements]
    public class GetLeadEcLoanResponse
    {
        public string PurposeId { get; set; }
        public string Purpose { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public decimal? Amount { get; set; }
        public string Term { get; set; }
        public string TermId { get; set; }
    }
}
