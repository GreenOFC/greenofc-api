using _24hplusdotnetcore.ModelDtos;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class CreateImportFileValidation : AbstractValidator<ImportFileDto>
    {
        public CreateImportFileValidation()
        {
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x => x.ImportType).IsInEnum();
            RuleFor(x => x.SaleInfomation).NotNull();
        }
    }
}
