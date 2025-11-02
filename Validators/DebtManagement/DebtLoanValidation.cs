using _24hplusdotnetcore.ModelDtos.DebtManagement;
using FluentValidation;

namespace _24hplusdotnetcore.Validators.DebtManagement
{
    public class DebtLoanValidation : AbstractValidator<DebtLoanDto>
    {
        public DebtLoanValidation()
        {
            RuleFor(x => x.DisbursementDate).NotNull();
            RuleFor(x => x.Term).NotNull().NotEmpty();
            RuleFor(x => x.Period).NotNull().NotEmpty();
            RuleFor(x => x.PaymentDueDate).NotNull();
        }
    }
}
