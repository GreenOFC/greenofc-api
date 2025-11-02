using _24hplusdotnetcore.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _24hplusdotnetcore.ModelDtos.CheckSims
{
    public class PtfScopingVerifyOtpRequest : PtfScopingBaseRequest
    {
        [Required]
        public PtfScoringMobileNetwork MobileNetwork { get; set; }

        public string IdCard { get; set; }

        public string Otp { get; set; }

        public IEnumerable<string> FrequentContacts { get; set; }
    }
}
