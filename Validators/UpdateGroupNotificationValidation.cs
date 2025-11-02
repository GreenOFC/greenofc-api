using FluentValidation;
using _24hplusdotnetcore.ModelDtos;

namespace _24hplusdotnetcore.Validators
{
    public class UpdateGroupNotificationValidation : AbstractValidator<UpdateGroupNotificationDto>
    {
        public UpdateGroupNotificationValidation()
        {
            RuleFor(x => x.GroupName).NotEmpty().NotNull();
        }
    }
}
