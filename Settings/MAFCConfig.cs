namespace _24hplusdotnetcore.Settings
{
    public class MAFCConfig
    {
        public string CallbackUserName { get; set; }
        public string CallbackPassword { get; set; }
        public string Host { get; set; }
        public string CronExpression { get; set; }
        public bool IsTestMode { get; set; }
        public MAFCAuthenConfig MasterData { get; set; }
        public MAFCAuthenConfig CheckCustomer { get; set; }
        public MAFCAuthenConfig DataEntry { get; set; }
        public MAFCAuthenConfig DataUpdate { get; set; }
        public MAFCAuthenConfig DataUpload { get; set; }
        public MAFCAuthenConfig DataUploadDefer { get; set; }
        public MAFCS37AuthenConfig S37 { get; set; }
    }

    public class MAFCAuthenConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class MAFCS37AuthenConfig: MAFCAuthenConfig
    {
        public string VendorCode { get; set; }
    }
}
