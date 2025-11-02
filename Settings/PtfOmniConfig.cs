namespace _24hplusdotnetcore.Settings
{
    public class PtfOmniConfig
    {
        public string Host { get; set; }
        public string ClientId { get; set; }
        public string SecretId { get; set; }
        public string CronExpression { get; set; }
        public int NumberOfRecordPerProcess { get; set; }
        public string SaleIdTSA { get; set; }
        public string SaleIdDSA { get; set; }
    }
}
