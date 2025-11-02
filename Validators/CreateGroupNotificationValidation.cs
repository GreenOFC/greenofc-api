using _24hplusdotnetcore.ModelDtos;
using FluentValidation;

namespace _24hplusdotnetcore.Validators
{
    public class CreateGroupNotificationValidation : AbstractValidator<CreateGroupNotificationDto>
    {
        public CreateGroupNotificationValidation()
        {
            RuleFor(x => x.GroupName).NotEmpty().NotNull();
        }
    }
}
