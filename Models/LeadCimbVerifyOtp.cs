using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;

namespace _24hplusdotnetcore.Models
{
    [BsonCollection(MongoCollection.LeadCimbVerifyOtp)]
    public class LeadCimbVerifyOtp: BaseDocument
    {
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerIdCard { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
