using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.EC;
using FluentValidation;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Validators
{
    public class ECOfferListValidation : AbstractValidator<ECOfferListDto>
    {
        public ECOfferListValidation()
        {
            List<string> conditions = new List<string>()
            {
                ECOfferType.Main,
                ECOfferType.Negative,
                ECOfferType.Postive,
                ECOfferType.TrueNegative
            };

            RuleFor(x => x.OfferType).Must(x => conditions.Contains(x)).WithMessage("Invalid offer type");
            RuleForEach(x => x.InsuranceList).SetValidator(new ECOfferInsuranceListValidation()).When(x => x.InsuranceList != null);
        }
    }
}
