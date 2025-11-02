using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _24hplusdotnetcore.Models.EC
{
    public class ECOfferInsuranceList
    {
        public string InsuranceType { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? InsuranceAmount { get; set; }

        public float? PercentageInsurance { get; set; }
        public string BaseCalculation { get; set; }
    }
}