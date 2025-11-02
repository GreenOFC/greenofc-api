using _24hplusdotnetcore.ModelDtos.F88;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.F88
{
    public interface IRestF88AuthenService
    {
        [Post("/auth/v1.1/login")]
        Task<F88LoginRestResponse> GetAuthAsync([Body] F88LoginRestRequest body);
    }
}
