using _24hplusdotnetcore.ModelDtos.Pos;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class RemoveUserFromPosValidation : AbstractValidator<RemoveUserFromPosDto>
    {
        public RemoveUserFromPosValidation()
        {
            RuleFor(x => x.PosId).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
        }
    }
}
