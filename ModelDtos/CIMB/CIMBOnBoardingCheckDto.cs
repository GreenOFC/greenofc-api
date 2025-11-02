namespace _24hplusdotnetcore.ModelDtos.CIMB
{
    public class CIMBOnBoardingCheckDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdNumber { get; set; }
        public bool? IsPhoneVerified { get; set; }
        public bool? IsEmailVerified { get; set; }
        public string PartnerAccountId { get; set; }
        public string OnboardProductType { get; set; }
    }
}
