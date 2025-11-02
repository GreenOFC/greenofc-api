using _24hplusdotnetcore.ModelDtos.F88;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.F88
{
    public interface IRestF88Service
    {
        [Post("/F88LadipageAPI")]
        Task<F88RestResponse> PostAsync(F88RestRequest request);
    }
}
