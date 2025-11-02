using _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeRest;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.PtfOmnis
{
    public interface IPtfCheckIncomeRestService
    {
        [Post("/v2/trusting-social/check-consent")]
        Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeCheckConsentRestResponse>> CheckConsentAsync([Body] PtfCheckIncomeCheckConsentRestRequest body);

        [Post("/v2/trusting-social/send-otp")]
        Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeSendOtpRestResponse>> SendOtpAsync([Body] PtfCheckIncomeSendOtpRestRequest body);

        [Post("/v2/trusting-social/verify-otp")]
        Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeVerifyOtpRestResponse>> VerifyOtpAsync([Body] PtfCheckIncomeVerifyOtpRestRequest body);

        [Post("/v2/trusting-social/income-score/query")]
        Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeFetchRestResponse>> QueryIncomeAsync([Body] PtfCheckIncomeCheckConsentRestRequest body);

        [Post("/v2/trusting-social/income-score/fetching-data")]
        Task<PtfCheckIncomeBaseRestResponse<PtfCheckIncomeFetchRestResponse>> FetchIncomeAsync([Body] PtfCheckIncomeFetchRestRequest body);
    }
}
