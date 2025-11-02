using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IRestMAFCCheckCustomerService
    {
        [Post("/3p/v2/check-customer")]
        // [Post("/public/api/v1/customer/CheckCustomer")]
        [Headers("Authorization: Basic")]
        Task<MAFCResponse<T>> CheckCustomerAsync<T>(MAFCCheckCustomerRestRequest request);

        // UAT
        // [Post("/3p/v3/check-customer")]
        // Production
        [Post("/check-customer/3p/v3/checkcustomer")]
        [Headers("Authorization: Basic")]
        Task<MAFCResponse<T>> CheckCustomerV3Async<T>(MAFCCheckCustomerV3RestRequest request);
    }
}
