using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class RegisterUserRequestDto
    {
        [Required(AllowEmptyStrings = false)]
        public string FullName { get; set; }

        public string IdCard { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string UserEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string UserPassword { get; set; }

        public string TeamLeadUserId { get; set; }

        public string PosId { get; set; }

        public string ReferralCode { get; set; }
    }
}
