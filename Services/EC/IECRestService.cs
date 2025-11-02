using _24hplusdotnetcore.ModelDtos.EC;
using Newtonsoft.Json.Linq;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.EC
{
    public interface IECRestService
    {
        // @todo
        // [Post("/los-united/v2/dsa/send-loan-application")]

        [Post("/api/loanRequestServices/v1/dsa/send-loan-application")]
        Task<JObject> SendFullLoan([Body] ECFullLoanDto request, [Header("Authorization")] string token);

        // [Post("/los-united/v1/dsa/offers")]
        [Post("/api/loanRequestServices/v1/dsa/offers")]
        Task<JObject> GetOfferList([Body] ECOfferDto request, [Header("Authorization")] string token);

        // [Post("/los-united/v2/dsa/select-offer")]
        [Post("/api/loanRequestServices/v1/dsa/select-offer")]
        Task<JObject> SelectOffer([Body] ECSelectOfferDto request, [Header("Authorization")] string token);

        // [Post("/los-united/v1/eligible/check")]
        [Post("/api/eligibleService/v1/eligible/check")]
        Task<JObject> CheckEligigle([Body] ECEligibleDto request, [Header("Authorization")] string token);

        // [Post("/los-united/v1/product-list")]
        [Post("/api/loanRequestServices/v1/product-list")]
        Task<JObject> GetProductList([Body] ECProductListDto request, [Header("Authorization")] string token);


        [Get("/los-united/v1/document/presinged-url")]
        Task<JObject> GetPresignUrl([AliasAs("partner_code")] string partnerCode, ECGetPresignUrl parameters, [Header("Authorization")] string token);
    }
}
