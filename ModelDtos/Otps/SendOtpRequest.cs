using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.ModelDtos.Otps
{
    public class SendOtpRequest
    {
        public string ReferenceId { get; set; }

        public string ReferenceType { get; set; }

        public string Fullname { get; set; }

        public string Identifier { get; set; }

        public VerifyType Type { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
