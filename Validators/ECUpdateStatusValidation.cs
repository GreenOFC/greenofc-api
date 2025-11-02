using _24hplusdotnetcore.ModelDtos.EC;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class ECUpdateStatusValidation : AbstractValidator<ECUpdateStatusDto>
    {
        public ECUpdateStatusValidation()
        {
            RuleFor(x => x.Data).NotNull().WithMessage("Data cannot be null").SetValidator(new ECUpdateStatusDataValidation());
        }
    }
}
