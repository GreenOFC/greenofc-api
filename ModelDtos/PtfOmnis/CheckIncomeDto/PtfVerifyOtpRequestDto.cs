using _24hplusdotnetcore.ModelDtos.CheckSims;

namespace _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeDto
{
    public class PtfVerifyOtpRequestDto : PtfScopingBaseRequest
    {
        public string IdCard { get; set; }
        public string Otp { get; set; }
    }
}
