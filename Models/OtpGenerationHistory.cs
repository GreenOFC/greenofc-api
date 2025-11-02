using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonCollection(MongoCollection.OtpGenerationHistory)]
    public class OtpGenerationHistory: BaseDocument
    {
        public string ReferenceId { get; set; }
        public string Identifier { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public string Otp { get; set; }

        public bool IsVerified { get; set; }
    }
}
