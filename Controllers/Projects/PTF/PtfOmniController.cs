using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.PtfOmnis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.PtfOmnis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [AllowAnonymous]
    [Route("api/ptf-omnis")]
    public class PtfOmniController : BaseController
    {
        private readonly ILogger<PtfOmniController> _logger;
        private readonly IPtfOmniService _ptfOmniService;
        private readonly IPtfOmniMasterDataService _ptfOmniMasterDataService;
        private readonly IPtfOmniDataProcessingService _ptfOmniDataProcessingService;

        public PtfOmniController(
            ILogger<PtfOmniController> logger,
            IPtfOmniService ptfOmniService,
            IPtfOmniMasterDataService ptfOmniMasterDataService,
            IPtfOmniDataProcessingService ptfOmniDataProcessingService)
        {
            _logger = logger;
            _ptfOmniService = ptfOmniService;
            _ptfOmniMasterDataService = ptfOmniMasterDataService;
            _ptfOmniDataProcessingService = ptfOmniDataProcessingService;
        }

        /// <summary>
        /// Lấy thông tin phiên bản của MDM
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /masterdatas/version
        ///     {
        ///
        ///     }
        ///
        /// </remarks>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("masterdatas/version")]
        public async Task<IActionResult> GetMasterDataVersionAsync(object body)
        {
            try
            {
                var response = await _ptfOmniService.GetMasterDataVersionAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy dữ liệu của MDM theo type
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /masterdatas/list
        ///     {
        ///         "type":"PTF_DOCUMENT_TYPE",
        ///         "getMetaData":true
        ///      }
        ///
        /// </remarks>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("masterdatas/list")]
        public async Task<IActionResult> GetMasterDataListAsync(PtfOmniMasterDataListRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetMasterDataListAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra CIF
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("fetchsvc/cif")]
        public async Task<IActionResult> GetFetchServiceCifAsync(PtfOmniFetchServiceCifRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetFetchServiceCifAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra khoản vay hiện tại
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("fetchsvc/cbs")]
        public async Task<IActionResult> GetFetchServiceCbsAsync(PtfOmniFetchServiceCbsRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetFetchServiceCbsAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra khoản vay đang xử lý
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("fetchsvc/los")]
        public async Task<IActionResult> GetFetchServiceLosAsync(PtfOmniFetchServiceLosRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetFetchServiceLosAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra danh sách đen
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("fetchsvc/blacklist")]
        public async Task<IActionResult> GetFetchServiceBlackListAsync(PtfOmniFetchServiceBlackListRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetFetchServiceBlackListAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Xóa chứng từ của 1 hồ sơ
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("documents")]
        public async Task<IActionResult> DeleteDocumentsAsync(PtfOmniDocumentDeleteRequest body)
        {
            try
            {
                var response = await _ptfOmniService.DeleteDocumentsAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách chứng từ của hồ sơ
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("documents/list")]
        public async Task<IActionResult> GetDocumentListAsync(PtfOmniDocumentListRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetDocumentListAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy link xem chứng từ hồ sơ
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("documents/doc-url")]
        public async Task<IActionResult> GetDocumentUrlAsync(PtfOmniDocumentUrlRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetDocumentUrlAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Tải lên chứng từ hồ sơ
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("documents/upload-multipart")]
        public async Task<IActionResult> UploadDocumentAsync([FromForm] PtfOmniDocumentUploadRequest body)
        {
            try
            {
                var response = await _ptfOmniService.UploadDocumentAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy mã CaseId khi tạo hồ sơ khoản vay
        /// </summary>
        /// <returns></returns>
        [HttpGet("loans/newcase")]
        public async Task<IActionResult> GetLoanNewCaseAsync()
        {
            try
            {
                var response = await _ptfOmniService.GetLoanNewCaseAsync();
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Tạo hồ sơ khoản vay
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("loans")]
        public async Task<IActionResult> CreateLoanAsync(PtfOmniLoanCreateRequest body)
        {
            try
            {
                var response = await _ptfOmniService.CreateLoanAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của 1 hồ sơ khoản vay
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        [HttpGet("loans")]
        public async Task<IActionResult> GetLoanDetailAsync(string caseId)
        {
            try
            {
                var response = await _ptfOmniService.GetLoanDetailAsync(caseId);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách hồ sơ khoản vay
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("loans/list")]
        public async Task<IActionResult> GetLoanListAsync(PtfOmniLoanListRequest body)
        {
            try
            {
                var response = await _ptfOmniService.GetLoanListAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật thông tin hồ sơ
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("loans")]
        public async Task<IActionResult> UpdateLoanAsync(PtfOmniLoanUpdateRequest body)
        {
            try
            {
                var response = await _ptfOmniService.UpdateLoanAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Chuyển bước hồ sơ khoản vay
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("loans/save-decision")]
        public async Task<IActionResult> SaveDecisionLoanAsync(PtfOmniLoanSaveDecisionRequest body)
        {
            try
            {
                var response = await _ptfOmniService.SaveDecisionLoanAsync(body);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Đồng bộ master data
        /// </summary>
        [HttpGet("sync-master-data")]
        public async Task<IActionResult> SaveDecisionLoanAsync()
        {
            try
            {
                await _ptfOmniMasterDataService.SyncAsync();
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra thông tin khách hàng có hợp lệ
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("{customerId}/check-valid-loan")]
        public async Task<IActionResult> CheckValidLoanAsync(string customerId)
        {
            try
            {
                var response = await _ptfOmniService.CheckValidLoanAsync(customerId);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra thông tin khách hàng có hợp lệ
        /// </summary>
        /// <returns></returns>
        [HttpPost("check-valid-loan")]
        public async Task<IActionResult> CheckValidLoanAsync(PtfOmniCheckValidLoanRequest request)
        {
            try
            {
                var response = await _ptfOmniService.CheckValidLoanAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Thêm / Cập nhật thông tin khoản vay qua PTF Omni
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("{customerId}/upsert-loan")]
        public async Task<IActionResult> UpsertLoanAsync(string customerId)
        {
            try
            {
                if (customerId == "all")
                {
                    await _ptfOmniDataProcessingService.SyncDataAsync();
                }
                else
                {
                    await _ptfOmniDataProcessingService.SyncDataAsync(customerId);
                }
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin master data theo filter
        /// </summary>
        /// <returns></returns>
        [HttpGet("master-data")]
        public async Task<IActionResult> CheckValidLoanAsync([FromQuery] PtfOmniMasterDataRequest request)
        {
            try
            {
                var response = await _ptfOmniMasterDataService.GetPtfOmniMasterDataAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin payloads theo filter
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = PermissionCost.PtfOmniGetPayload)]
        [HttpGet("payloads")]
        public async Task<IActionResult> GetPayloadAsync([FromQuery] PtfOmniGetDataProcessingRequest ptfOmniGetDataProcessingRequest)
        {
            try
            {
                var response = await _ptfOmniDataProcessingService.GetAsync(ptfOmniGetDataProcessingRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Lấy thông tin payloads theo filter
        /// </summary>
        /// <returns></returns>
        // [Authorize(Roles = PermissionCost.PtfOmniGetPayload)]
        [HttpPut("{id}/resend-payload")]
        public async Task<IActionResult> UpadatePayloadAsync(string id, PtfOmniUpdateDataProcessingRequest dto)
        {
            try
            {
                await _ptfOmniDataProcessingService.UpdateAsync(id, dto);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Đồng bộ data từ PTF về
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns></returns>
        [Authorize(Roles = PermissionCost.PtfOmniSyncLoan)]
        [HttpGet("{customerId}/sync-loan")]
        public async Task<IActionResult> UpdateCustomerStatusAsync(string customerId)
        {
            try
            {
                await _ptfOmniService.SyncLoanAsync(customerId);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Đồng bộ All data từ PTF về
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = PermissionCost.PtfOmniSyncAllLoan)]
        [HttpGet("sync-all-processing-customer")]
        public async Task<IActionResult> UpdateCustomerStatusAsync()
        {
            try
            {
                await _ptfOmniService.UpdateCustomerStatusAsync();
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Hủy hồ sơ
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <param name="ptfOmniCancelApplicationRequest">Lý do</param>
        /// <returns></returns>
        [HttpPut("{customerId}/cancel-loan")]
        public async Task<IActionResult> CancelLoanApplicationAsync(string customerId, PtfOmniCancelApplicationRequest ptfOmniCancelApplicationRequest)
        {
            try
            {
                await _ptfOmniService.CancelLoanApplicationAsync(customerId, ptfOmniCancelApplicationRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
