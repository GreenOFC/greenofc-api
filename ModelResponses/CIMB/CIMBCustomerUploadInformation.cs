namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public class CIMBCustomerUploadInformation
    {
        public string CertFrontPicUri { get; set; }
        public string CertFrontPicName { get; set; }
        public byte[] CertFrontPicBytes { get; set; }
        public string CertBackPicUri { get; set; }
        public string CertBackPicName { get; set; }
        public byte[] CertBackPicBytes { get; set; }
        public string SelfieUri { get; set; }
        public string SelfieName { get; set; }
        public byte[] SelfieBytes { get; set; }
    }
}
