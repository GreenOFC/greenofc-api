using _24hplusdotnetcore.ModelDtos.Pos;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class UpdateUserToPosValidation :  AbstractValidator<UpdateUserPosDto>
    {
        public UpdateUserToPosValidation()
        {
            RuleFor(x => x.PosId).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
        }
    }
}
