namespace _24hplusdotnetcore.Settings
{
    public class CIMBConfig
    {
        public string Host { get; set; }
        public string APIKey { get; set; }

        public string PartnerId { get; set; }
        public string KeyId { get; set; }
        public string PublicKey { get; set; }
        public string XIdentifier { get; set; }

        public string LoanStatusCronExpression { get; set; }
        public string CronExpression { get; set; }

        public string CertFilePath { get; set; }
        public string CertPassword { get; set; }
    }
}
