
using System;

namespace _24hplusdotnetcore.ModelDtos.F88
{
    public class GetF88NotiResponse
    {
        public string TransactionId { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string DetailStatus { get; set; }
        public string LoanAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
