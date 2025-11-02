using _24hplusdotnetcore.ModelDtos.Users;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class UpdateCurrentUserValidation: AbstractValidator<UpdateCurrentUserRequest>
    {
        public UpdateCurrentUserValidation()
        {
            RuleFor(x => x.IdCard).NotEmpty().NotNull();
            RuleFor(x => x.IdCardDate).NotEmpty().NotNull();
            RuleFor(x => x.IdCardProvinceId).NotEmpty().NotNull();
            RuleFor(x => x.ResidentAddress).NotEmpty().NotNull();
            RuleFor(x => x.TemporaryAddress).NotEmpty().NotNull();
            RuleFor(x => x.TemporaryAddress).NotEmpty().NotNull();
            RuleFor(x => x.Working).SetValidator(new UserWorkingValidation());
        }
    }

    public class UserWorkingValidation : AbstractValidator<UserWorkingDto>
    {
        public UserWorkingValidation()
        {
            RuleFor(x => x.CompanyAddress).NotEmpty().NotNull();
        }
    }
}
