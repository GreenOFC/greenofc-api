using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IRestMAFCS37Service
    {
        [Post("/cic/submit-s37")]
        [Headers("Authorization: Basic")]
        Task<T> SubmitAsync<T>(MAFCSubmitS37RestRequest request);

        [Post("/cic/polling-s37")]
        [Headers("Authorization: Basic")]
        Task<T> PollingAsync<T>(MAFCPollingS37RestRequest request);
    }
}
