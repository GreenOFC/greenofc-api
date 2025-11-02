namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class CreateMafcStep1Request
    {
        public string OldCustomerId { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeId { get; set; }
        public string UserName { get; set; }
        public string ProductLine { get; set; }
        public string MobileVersion { get; set; }
        public MafcPersonalDto Personal { get; set; }
    }
}
