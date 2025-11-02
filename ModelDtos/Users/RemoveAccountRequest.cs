using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class RemoveAccountRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string UserPassword { get; set; }
    }
}
