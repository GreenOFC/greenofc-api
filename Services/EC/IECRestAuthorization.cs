using Newtonsoft.Json.Linq;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public interface IECRestAuthorization
    {
        // //@todo
        // [Headers("Authorization: Basic", "Content-Type: application/x-www-form-urlencoded")]
        // [Post("/aaa/v03/oauth2/token")]
        // Task<JObject> GetToken([Body(BodySerializationMethod.UrlEncoded)] FormUrlEncodedContent content);

        [Headers("Authorization: Basic")]
        [Post("/api/uaa/oauth/token?grant_type=client_credentials")]
        Task<JObject> GetToken();
    }
}
