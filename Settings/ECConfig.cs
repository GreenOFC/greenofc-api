namespace _24hplusdotnetcore.Settings
{
    public class ECConfig
    {
        public string Host { get; set; }
        public string UAT { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AppClientId { get; set; }
        public string AppSecretId { get; set; }
        public string CronExpression { get; set; }
        public string PartnerCode { get; set; }
        public string Channel { get; set; }
        public string SaleCode { get; set; }

        public SFTP SFTP { get; set; }
    }

    public class SFTP
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
    }
}
