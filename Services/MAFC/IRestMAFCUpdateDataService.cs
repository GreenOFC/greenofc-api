using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IRestMAFCUpdateDataService
    {
        [Post("/3p/v2/data-entry-update")]
        [Headers("Authorization: Basic")]
        Task<MAFCResponse<T1>> PostAsync<T1, T2>(T2 request);
    }
}
