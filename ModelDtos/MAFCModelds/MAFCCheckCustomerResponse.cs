namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCCheckCustomerResponse
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Partner { get; set; }
        public int? StatusNumber { get; set; }
        public string Message { get; set; }
    }
    public class MAFCCheckCustomerV3Response
    {
        public string Phone { get; set; }
        public string CustomerName { get; set; }
        public string CMND { get; set; }
        public int? StatusNumber { get; set; }
        public string Message { get; set; }
    }
}
