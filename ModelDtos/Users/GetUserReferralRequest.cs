using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class GetUserReferralRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
    }
}
