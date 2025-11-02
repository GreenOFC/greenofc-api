using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class ChangeUserStatusRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public bool IsActive { get; set; }
    }
}
