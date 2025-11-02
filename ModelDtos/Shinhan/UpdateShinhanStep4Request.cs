namespace _24hplusdotnetcore.ModelDtos.Shinhan
{
    public class UpdateShinhanStep4Request
    {
        public ShinhanAddressDto ResidentAddress { get; set; }
        public ShinhanAddressDto TemporaryAddress { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
    }
}
