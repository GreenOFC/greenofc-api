using _24hplusdotnetcore.ModelDtos.EC;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class ECUpdateStatusDataValidation : AbstractValidator<ECUpdateStatusDataDto>
    {
        public ECUpdateStatusDataValidation()
        {
            RuleForEach(x => x.OfferList).SetValidator(new ECOfferListValidation()).When( x=> x.OfferList != null);
        }
    }
}
