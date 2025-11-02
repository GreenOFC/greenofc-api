using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.EC;
using FluentValidation;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Validators
{
    public class ECOfferInsuranceListValidation : AbstractValidator<ECOfferInsuranceListDto>
    {
        public ECOfferInsuranceListValidation()
        {
            List<string> conditions = new List<string>()
            {
                ECBaseCalculation.RA.ToString(),
                ECBaseCalculation.RANI.ToString(),
            };

            RuleFor(x => x.BaseCalculation).Must(x => conditions.Contains(x)).WithMessage("Invalid base calculation");
        }
    }
}
