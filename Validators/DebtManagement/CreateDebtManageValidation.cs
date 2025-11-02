using _24hplusdotnetcore.ModelDtos.DebtManagement;
using FluentValidation;

namespace _24hplusdotnetcore.Validators.DebtManagement
{
    public class CreateDebtManageValidation : AbstractValidator<CreateDebtDto>
    {
        public CreateDebtManageValidation()
        {
            RuleFor(x => x.ContractCode).NotEmpty().NotEmpty();
            RuleFor(x => x.GreenType).NotEmpty().NotEmpty();
            RuleFor(x => x.Loan).SetValidator(new DebtLoanValidation());
            RuleFor(x => x.Personal).SetValidator(new DebtPersonalValidation());
            RuleFor(x => x.SaleInfo).SetValidator(new DebtSaleInfoValidation());
        }
    }
}
