using Refit;

namespace _24hplusdotnetcore.ModelDtos.EC
{
    public class ECTokenDto
    {
        [AliasAs("client_credentials")]
        public string ClientCredentials { get; set; }
    }
}
