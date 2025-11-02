namespace _24hplusdotnetcore.ModelDtos.MC
{
    public class UpdateMcStep4Request
    {
        public McAddressDto ResidentAddress { get; set; }
        public McAddressDto TemporaryAddress { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
    }
}
