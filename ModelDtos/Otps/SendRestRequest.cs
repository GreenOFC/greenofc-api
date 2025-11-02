using _24hplusdotnetcore.Common.Enums;

namespace _24hplusdotnetcore.ModelDtos.Otps
{
    public class SendRestRequest
    {
        public string Fullname { get; set; }

        public string Identifier { get; set; }

        public VerifyType Type { get; set; }

        public string Value { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
