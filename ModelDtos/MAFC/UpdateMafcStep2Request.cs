namespace _24hplusdotnetcore.ModelDtos.MAFC
{
    public class UpdateMafcStep2Request
    {

        public MafcAddressDto ResidentAddress { get; set; }
        public MafcAddressDto TemporaryAddress { get; set; }
        public bool IsTheSameResidentAddress { get; set; }
        public string FamilyBookNo { get; set; }
    }
}
