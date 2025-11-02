using _24hplusdotnetcore.ModelDtos.DebtManagement;
using FluentValidation;

namespace _24hplusdotnetcore.Validators.DebtManagement
{
    public class DebtSaleInfoValidation : AbstractValidator<DebtSaleInfoDto>
    {
        public DebtSaleInfoValidation()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty();
        }
    }
}
