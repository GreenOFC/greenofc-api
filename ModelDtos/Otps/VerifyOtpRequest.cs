namespace _24hplusdotnetcore.ModelDtos.Otps
{
    public class VerifyOtpRequest
    {
        public string ReferenceId { get; set; }

        public string ReferenceType { get; set; }

        public string Fullname { get; set; }

        public string Identifier { get; set; }

        public string Otp { get; set; }
    }
}
