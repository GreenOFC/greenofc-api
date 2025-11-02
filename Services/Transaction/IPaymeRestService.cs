using Newtonsoft.Json.Linq;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Transaction
{
    public interface IPaymeRestService
    {
        [Post("/payment/web")]
        Task<HttpResponseMessage> CreateOrder(
            [Header("x-api-client")] string apiClient, 
            [Header("x-api-key")] string apiKey, 
            [Header("x-api-action")] string apiAction, 
            [Header("x-api-validate")] string apiValidate,
            StringContent content);

        [Post("/payment/refund")]
        Task<HttpResponseMessage> Refund(
            [Header("x-api-client")] string apiClient,
            [Header("x-api-key")] string apiKey,
            [Header("x-api-action")] string apiAction,
            [Header("x-api-validate")] string apiValidate,
            StringContent content);
    }
}
