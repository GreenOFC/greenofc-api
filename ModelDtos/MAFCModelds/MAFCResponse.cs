namespace _24hplusdotnetcore.ModelDtos.MAFCModelds
{
    public class MAFCResponse<T>
    {
        public string Success { get; set; }
        public string Message { get; set; }
        public string TrNo { get; set; }
        public T Data { get; set; }
    }
}
