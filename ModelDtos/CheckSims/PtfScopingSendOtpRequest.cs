using _24hplusdotnetcore.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScopingSendOtpRequest : PtfScopingBaseRequest
    {
        [Required]
        public PtfScoringMobileNetwork MobileNetwork { get; set; }

        public string IdCard { get; set; }
    }
}
