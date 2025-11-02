using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IRestMAFCMasterDataService
    {
        [Post("/thirdparty/dataentryMD")]
        // [Post("/masterdatamci")]
        [Headers("Authorization: Basic")]
        Task<MAFCResponse<T>> GetAsync<T>(MAFCMasterDataRequest request);
    }
}
