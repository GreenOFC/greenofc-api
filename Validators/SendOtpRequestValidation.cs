using _24hplusdotnetcore.ModelDtos.MC;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class SendOtpRequestValidation : AbstractValidator<SendOtpRequest>
    {
        public SendOtpRequestValidation()
        {
            RuleFor(x => x.RequestedMsisdn).NotNull().Length(10).WithMessage("Số điện thoại");
            RuleFor(x => x.TypeScore).NotNull().MaximumLength(12).WithMessage("Nhà mạng");
        }
    }
}
