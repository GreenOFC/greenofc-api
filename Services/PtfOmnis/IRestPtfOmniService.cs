using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using Refit;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.PtfOmnis
{
    public interface IRestPtfOmniService
    {
        [Post("/masterdatas/version")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniMasterDataVersionResponse>>> GetMasterDataVersionAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] object body);

        [Post("/masterdatas/list")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniMasterDataListResponse>>> GetMasterDataListAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniMasterDataListRequest body);

        [Post("/fetchsvc/cif")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCifResponse>>> GetFetchServiceCifAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniFetchServiceCifRequest body);

        [Post("/fetchsvc/cbs")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCbsResponse>>> GetFetchServiceCbsAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniFetchServiceCbsRequest body);

        [Post("/fetchsvc/los")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceLosResponse>>> GetFetchServiceLosAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniFetchServiceLosRequest body);

        [Post("/fetchsvc/blacklist")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceBlackListResponse>>> GetFetchServiceBlackListAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniFetchServiceBlackListRequest body);

        [Delete("/documents")]
        Task<PtfOmniResponseModel<object>> DeleteDocumentsAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniDocumentDeleteRequest body);

        [Post("/documents/list")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniDocumentListResponse>>> GetDocumentListAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniDocumentListRequest body);

        [Post("/documents/doc-url")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniDocumentUrlResponse>>> GetDocumentUrlAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniDocumentUrlRequest body);

        [Multipart]
        [Post("/documents/upload-multipart")]
        Task<PtfOmniDocumentUploadResponse> UploadDocumentAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, string caseId, string documentCategory, string documentType, [AliasAs("file")] StreamPart stream);

        [Get("/loans/newcase")]
        Task<PtfOmniResponseModel<PtfOmniLoanNewCaseResponse>> GetLoanNewCaseAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey);

        [Post("/loans")]
        Task<PtfOmniResponseModel<object>> CreateLoanAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniLoanCreateRequest body);

        [Get("/loans")]
        Task<PtfOmniResponseModel<PtfOmniLoanDetailResponse>> GetLoanDetailAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, string caseId);

        [Post("/loans/list")]
        Task<PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniLoanListResponse>>> GetLoanListAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniLoanListRequest body);

        [Put("/loans")]
        Task<PtfOmniResponseModel<object>> UpdateLoanAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniLoanUpdateRequest body);

        [Post("/loans/save-decision")]
        Task<PtfOmniResponseModel<object>> SaveDecisionLoanAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniLoanSaveDecisionRequest body);

        [Post("/loans/change-status")]
        Task<PtfOmniResponseModel<object>> ChangeStatusLoanAsync([Header("X-CLIENT-ID")] string clientId, [Header("X-SECRET-API")] string secretKey, [Body] PtfOmniLoanChangeStatusRequest body);
    }
}
