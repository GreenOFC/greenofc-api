using _24hplusdotnetcore.ModelDtos.Otps;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IOtpRestService
    {
        [Post("/api/v1/otps/send")]
        Task<SendRestResponse> SendAsync([Body] SendRestRequest body);

        [Post("/api/v1/otps/verify")]
        Task<VerifyRestResponse> VerifyAsync([Body] VerifyRestRequest body);
    }
}
