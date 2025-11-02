using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.LeadCimbs
{
    public class CimbSendVerifyRequest
    {
        [Required]
        [EnumDataType(typeof(VerifyType))]
        public VerifyType? Type { get; set; }
    }
}
