using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UserSendVerifyRequest
    {
        [Required]
        public string UserId { get; set; }
    }
}
