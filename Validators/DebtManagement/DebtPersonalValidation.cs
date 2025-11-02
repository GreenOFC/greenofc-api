using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.DebtManagement;
using FluentValidation;

namespace _24hplusdotnetcore.Validators.DebtManagement
{
    public class DebtPersonalValidation : AbstractValidator<DebtPersonalDto>
    {
        public DebtPersonalValidation()
        {
            RuleFor(x => x.IdCard).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Phone).NotNull().NotEmpty().Length(10, 10).WithMessage(DebtManageMessage.InvalidPhoneNumber);
        }   
    }
}
