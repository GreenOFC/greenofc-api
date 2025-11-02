using _24hplusdotnetcore.ModelDtos.CIMB;
using _24hplusdotnetcore.ModelDtos.LeadCimbs;
using _24hplusdotnetcore.ModelResponses.CIMB;
using _24hplusdotnetcore.Models.CIMB;
using Newtonsoft.Json.Linq;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.CIMB
{
    public interface ICIMBRestService
    {
        [Post("/24hplus/v3/authentication-ms/partners/authenticate")]
        Task<CIMBAuthenticationToken> GetToken();

        [Get("/24hplus/v3/authentication-ms/api/rsa-public")]
        Task<CIMBAuthenticationToken> GetRsaKey();

        [Post("/24hplus/v3/onboarding-ms/partners/loan-onboarding/submit")]
        Task<string> SubmitLoan(
            [Body] byte[] cimbSubmitDto, 
            [Header("Authorization")] string token,
            [Header("x-encryption-key")] string key,
            [Header("x-identifier")] string xIdentifier,
            [Header("Content-Type")] string contentType = "application/octet-stream"
            );

        [Multipart]
        [Post("/24hplus/v3/onboarding-ms/partners/loan-onboarding/upload")]
        Task<CIMBCustomerUploadResult> UploadCustomerProfile(
            [AliasAs("loanId")] string loanId,
            [AliasAs("partnerAccountId")] string partnerAccountId,
            [AliasAs("CERT_FRONT_PIC")] ByteArrayPart certFrontPicBytes,
            [AliasAs("CERT_BACK_PIC")] ByteArrayPart certBackPicBytes,
            [AliasAs("SELFIE")] ByteArrayPart sefieBytes,
            [Header("Authorization")] string token,
            [Header("x-identifier")] string xIdentifier
            );

        [Post("/24hplus/v3/onboarding-ms/partners/accounts/check-onboarding-permission")]
        Task<JObject> CheckOnBoarding([Body] CIMBOnBoardingCheckDto request);

        [Post("/24hplus/v3/onboarding-ms/partners/accounts/casa-onboarding/status")]
        Task<string> GetLoanStatus(
            [Body] byte[] body,
            [Header("Authorization")] string token,
            [Header("x-identifier")] string xIdentifier,
            [Header("x-encryption-key")] string xEncryptionKey,
            [Header("Content-Type")] string contentType = "application/octet-stream"
           );

        [Post("/24hplus/v3/resource-ms/partners/resources")]
        Task<GetCimbResourceRestResponse> GetResourceAsync([Body] GetCimbResourceRestRequest request);
    }
}
