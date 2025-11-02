namespace _24hplusdotnetcore.ModelDtos.Sms
{
    public class SmsRequest
    {
        public string ClientNo { get; set; }
        public string ClientPass { get; set; }
        public string SenderName { get; set; }
        public string PhoneNumber { get; set; }
        public string SmsMessage { get; set; }
        public string SmsGUID { get; set; }
        public string ServiceType { get; set; }
    }
}
