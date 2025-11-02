using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class RegisterUserRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        public string FullName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string UserEmail { get; set; }

        public string Uuid { get; set; }
        public string Ostype { get; set; }
        public string MobileVersion { get; set; }
        public string Registration_token { get; set; }
    }
}
