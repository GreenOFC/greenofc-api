using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    [BsonIgnoreExtraElements]
    public class CimbLoanDto: IValidatableObject
    {
        [Range(5000000, 100000000)]
        public decimal? Amount { get; set; }
        public string Term { get; set; }
        public string TermId { get; set; }
        public string Purpose { get; set; }
        public string PurposeId { get; set; }
        public string InterestRatePerYear { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount % 100 != 0)
            {
                yield return new ValidationResult("Số tiền vay phải là bội số của 100.000", new string[] { nameof(Amount) });
            }
        }
    }
}
