namespace _24hplusdotnetcore.Models.CIMB
{
    public class CIMBAuthenticationToken
    {
        public string SystemCode { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }

    }

    public class Data
    {
        public string AccessToken { get; set; }
        public string AccessTokenHash { get; set; }
        public string Key { get; set; }
    }
}
