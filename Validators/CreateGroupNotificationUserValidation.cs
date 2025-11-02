using _24hplusdotnetcore.ModelDtos;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class CreateGroupNotificationUserValidation : AbstractValidator<CreateGroupNotificationUserDto>
    {
        public CreateGroupNotificationUserValidation()
        {
            RuleFor(x => x.UserId).NotEmpty().NotEmpty();
            RuleFor(x => x.GroupNotificationCode).NotEmpty().NotEmpty();
        }
    }
}
