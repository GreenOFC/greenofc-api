using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Models.EC
{
    public class ECOfferList
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? OfferId { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? OfferAmount { get; set; }

        public float? InterestRate { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? MonthlyInstallment { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? OfferTenor { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? MinFinancedAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? MaxFinancedAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? OfferVariant { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? OfferVar { get; set; }

        public string OfferType { get; set; }

        public IEnumerable<ECOfferInsuranceList> InsuranceList { get; set; }
    }
}
