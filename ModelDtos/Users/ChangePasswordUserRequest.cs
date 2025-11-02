using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class ChangePasswordUserRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string OldPassword { get; set; }
    }
}
