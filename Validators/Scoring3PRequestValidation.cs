using _24hplusdotnetcore.ModelDtos.MC;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class Scoring3PRequestValidation : AbstractValidator<Scoring3PRequest>
    {
        public Scoring3PRequestValidation()
        {
            RuleFor(x => x.PrimaryPhone).NotNull().NotEmpty().Length(10).WithMessage("Số điện thoại");
            RuleFor(x => x.TypeScore).NotNull().MaximumLength(12).WithMessage("Nhà mạng");
            RuleFor(x => x.NationalId).NotNull().MaximumLength(12).WithMessage("CMND/CCCD");
            RuleFor(x => x.VerificationCode).NotNull().NotEmpty().WithMessage("OTP");
        }     
    }
}