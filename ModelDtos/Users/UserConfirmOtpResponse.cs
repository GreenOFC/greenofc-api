namespace _24hplusdotnetcore.ModelDtos.Users
{
    public class UserConfirmOtpResponse
    {
        public UserConfirmOtpResponse(string verifyCode)
        {
            VerifyCode = verifyCode;
        }

        public string VerifyCode { get; set; }
    }
}
