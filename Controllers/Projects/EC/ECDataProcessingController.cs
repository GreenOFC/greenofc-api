using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.EC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.EC
{
    [Route("api/ec")]
    public class ECDataProcessingController : BaseController
    {
        private readonly ILogger<ECDataProcessingController> _logger;
        private readonly ECDataProcessingService _ecDataProcessingService;
        private readonly ILeadEcResourceService _leadEcResourceService;
        private readonly ECAuthorizationService _ecAuthorizationService;
        private readonly ILeadEcProductService _leadEcProductService;

        private readonly ECOfferService _ecOfferService;

        private readonly ECCustomerUploadFileService _ecCustomerUploadFileService;

        public ECDataProcessingController(
            ILogger<ECDataProcessingController> logger,
            ECDataProcessingService ecDataProcessingService,
            ECAuthorizationService ecAuthorizationService,
            ECOfferService ecOfferService,
            ECCustomerUploadFileService ecCustomerUploadFileService,
            ILeadEcProductService leadEcProductService,
            ILeadEcResourceService leadEcResourceService
            )
        {
            _logger = logger;
            _ecDataProcessingService = ecDataProcessingService;
            _ecAuthorizationService = ecAuthorizationService;
            _ecOfferService = ecOfferService;
            _ecCustomerUploadFileService = ecCustomerUploadFileService;
            _leadEcProductService = leadEcProductService;
            _leadEcResourceService = leadEcResourceService;
        }

        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<ActionResult> GetToken()
        {
            try
            {
                var result = await _ecAuthorizationService.GetToken();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("offers")]
        public async Task<ActionResult> CreateOfferList([FromBody] ECOfferDataDto request)
        {
            try
            {
                var result = await _ecOfferService.CreateOffer(request);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("eligible/check")]
        public async Task<ActionResult> CheckEligigle(string customerId)
        {
            try
            {
                var result = await _ecDataProcessingService.CheckEligigle(customerId);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("eligible/remote-checking")]
        public async Task<ActionResult> CheckRemoteEligigle(ECEligibleDto request)
        {
            try
            {
                var result = await _ecDataProcessingService.RemoteCheckingEligigle(request);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("loan/submit-json")]
        public async Task<ActionResult> SubmitLoanJson(string json)
        {
            try
            {
                var response = await _ecDataProcessingService.SendFullLoanTest(json);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost("loan/submit")]
        public async Task<ActionResult> SubmitLoan(string customerId)
        {
            try
            {
                var response = await _ecDataProcessingService.SendFullLoan(customerId);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("offer/select")]
        public async Task<ActionResult> SelectOffer(ECSelectOfferDto request)
        {
            try
            {
                var response = await _ecDataProcessingService.SelectOffer(request);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("upload-file")]
        public async Task<ActionResult> SelectOffer(string customerId)
        {
            try
            {
                await _ecCustomerUploadFileService.UploadCusomterUploadProfile(customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("product-list")]
        public async Task<ActionResult> GetProductList()
        {
            try
            {
                var response = await _ecDataProcessingService.GetProductList();
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpPost("sync-product")]
        public async Task<ActionResult> SyncEc()
        {
            try
            {
                await _leadEcProductService.SyncAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [ECAuhotirzation]
        [AllowAnonymous]
        [HttpPost("update-status")]
        public async Task<ActionResult> UpdateStatus([FromBody] ECUpdateStatusDto request)
        {
            try
            {
                var response = await _ecDataProcessingService.UpdateStatus(request);

                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpGet("update-master-data")]
        public async Task<ActionResult> UpdateMasterData()
        {
            try
            {
                await _leadEcResourceService.SyncAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [Authorize(Roles = PermissionCost.EcDataProsessingViewAll)]
        [HttpGet("{customerId}/data-processing")]
        public async Task<IActionResult> GetDataProsessingByCustomerId(string customerId)
        {
            try
            {
                if(customerId.IsEmpty())
                {
                    return BadRequest();
                }

                var response = await _ecDataProcessingService.GetDataProsessingByCustomerId(customerId);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [Authorize(Roles = PermissionCost.EcOfferListViewAll)]
        [HttpGet("offer-list")]
        public async Task<IActionResult> GetOfferList([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var response = await _ecOfferService.GetEcOfferList(pagingRequest);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

    }
}
