using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class RejectUserRequest
    {
        [Required]
        public string Reason { get; set; }
    }
}
