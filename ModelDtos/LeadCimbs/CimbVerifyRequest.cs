using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class CimbVerifyRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [EnumDataType(typeof(VerifyType))]
        public VerifyType? Type { get; set; }
    }
}
