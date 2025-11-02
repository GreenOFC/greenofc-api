using _24hplusdotnetcore.ModelDtos.CheckSims;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.CheckSims
{
    public interface IPtfScoringVnptRestService
    {
        [Post("/v1/vnpt-lending/check-mnp")]
        Task<PtfScoringBaseRestResponse<PtfScoringCheckMnpRestResponse>> CheckMnpAsync([Body] PtfScoringCheckConsentRestRequest body);

        [Post("/v1/vnpt-lending/send-otp")]
        Task<dynamic> SendOtpAsync([Body] PtfScoringSendOtpRestRequest body);

        [Post("/v1/vnpt-lending/verify-otp")]
        Task<dynamic> VerifyOtpAsync([Body] PtfScoringVerifyOtpVnptRestRequest body);

        [Post("/v1/vnpt-lending/credit-score/query")]
        Task<dynamic> QueryCreditScoreAsync([Body] PtfScoringQueryCreditScoreRestRequest body);

        [Post("/v1/vnpt-lending/credit-score/fetching-data")]
        Task<dynamic> FetchCreditScoreAsync([Body] PtfScoringFetchVNPTCreditScoreRestRequest body);

        [Post("/v1/vnpt-lending/fraud-score/query")]
        Task<dynamic> QueryFraudScoreAsync([Body] PtfScoringQueryFraudScoreRestRequest body);

        [Post("/v1/vnpt-lending/fraud-score/fetching-data")]
        Task<dynamic> FetchFraudScoreAsync([Body] PtfScoringVnptFetchFraudScoreRestRequest body);
    }
}
