using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class UpdateLeadCimbStep2Request: IValidatableObject
    {
        [Required]
        public CimbWorkingDto Working { get; set; }
        [Required]
        public IEnumerable<CimbReferenceDto> Referees { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(this.Referees?.GroupBy(x=>x.RelationshipId)?.Count() < this.Referees?.Count())
            {
                yield return new ValidationResult("Mối quan hệ của người tham chiếu 1 không được giống với người tham chiếu 2", new string[] { nameof(Referees) });
            }
        }
    }
}
