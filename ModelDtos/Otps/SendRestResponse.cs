namespace _24hplusdotnetcore.ModelDtos.Otps
{
    public class SendRestResponse
    {
        public string Message { get; set; }
        public SendOtpResult Results { get; set; }
    }

    public class SendOtpResult
    {
        public string Code { get; set; }
        public string VerifiedOtp { get; set; }
    }
}
