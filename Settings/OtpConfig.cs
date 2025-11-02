namespace _24hplusdotnetcore.Settings
{
    public class OtpConfig
    {
        public string Host { get; set; }
        public bool IsTestMode { get; set; }
        public string TestModeCode { get; set; }
        public string XapiKey { get; set; }
        public string VerifyBy { get; set; }
        public int NumberOfCharacters { get; set; }
        public long ExpireTime { get; set; }
        public long NumberOfSmsPerDay { get; set; }
        public string SmsMessageTemplate { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBodyTemplate { get; set; }

        public int ExpireTimeInMinutes => (int)(ExpireTime / 60000);
    }
}
