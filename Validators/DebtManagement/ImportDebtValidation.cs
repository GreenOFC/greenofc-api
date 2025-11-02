using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.DebtManagement;
using FluentValidation;

namespace _24hplusdotnetcore.Validators.DebtManagement
{
    public class ImportDebtValidation : AbstractValidator<ImportDebtDto>
    {
        public ImportDebtValidation()
        {
            RuleFor(x => x.ContractCode).NotEmpty().WithMessage(DebtManageMessage.InvalidContractCode);
            RuleFor(x => x.IdCard).Length(9, 12).When(x => !string.IsNullOrEmpty(x.IdCard)).WithMessage(DebtManageMessage.InvalidIdCard);
            RuleFor(x => x.Phone).Length(10, 10).When(x => !string.IsNullOrEmpty(x.Phone)).WithMessage(DebtManageMessage.InvalidPhoneNumber);
            RuleFor(x => x.Code).NotEmpty().WithMessage(DebtManageMessage.InvalidSaleCode);
        }
    }
}