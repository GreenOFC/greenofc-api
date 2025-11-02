using _24hplusdotnetcore.ModelDtos.CheckSims;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.CheckSims
{
    public interface IPtfScoringTrustingSocialRestService
    {
        [Post("/v1/trusting-social/check-consent")]
        Task<PtfScoringBaseRestResponse<PtfScoringCheckConsentRestResponse>> CheckConsentAsync([Body] PtfScoringCheckConsentRestRequest body);

        [Post("/v1/trusting-social/send-otp")]
        Task<PtfScoringBaseRestResponse<PtfScoringSendOtpRestResponse>> SendOtpAsync([Body] PtfScoringSendOtpRestRequest body);

        [Post("/v1/trusting-social/verify-otp")]
        Task<dynamic> VerifyOtpAsync([Body] PtfScoringVerifyOtpRestRequest body);

        [Post("/v1/trusting-social/credit-score/query")]
        Task<dynamic> QueryCreditScoreAsync([Body] PtfScoringQueryCreditScoreRestRequest body);

        [Post("/v1/trusting-social/credit-score/fetching-data")]
        Task<dynamic> FetchCreditScoreAsync([Body] PtfScoringFetchCreditScoreRestRequest body);

        [Post("/v1/trusting-social/fraud-score/query")]
        Task<dynamic> QueryFraudScoreAsync([Body] PtfScoringQueryFraudScoreRestRequest body);

        [Post("/v1/trusting-social/fraud-score/fetching-data")]
        Task<dynamic> FetchFraudScoreAsync([Body] PtfScoringTrustingSocialFetchFraudScoreRestRequest body);
    }
}
