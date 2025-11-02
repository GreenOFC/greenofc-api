using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CIMB;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CIMB;
using _24hplusdotnetcore.Services.CIMB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.CIMB
{
    [Route("api/cimb")]
    public class CIMBDataProcessingController : BaseController
    {
        private readonly ILogger<CIMBDataProcessingController> _logger;
        private readonly CIMBDataProcessingService _cimbDataProcessingService;
        private readonly CIMBService _cimbService;
        private readonly OnBoardingCheckingProcessingService _cimbOnBoardingCheckingProcessingService;
        private readonly CIMBCustomerLoanStatusService _cimbCustomerLoanStatusService;

        public CIMBDataProcessingController(
            ILogger<CIMBDataProcessingController> logger,
            CIMBDataProcessingService cimbDataProcessingService,
            CIMBService cimbService,
            OnBoardingCheckingProcessingService cimbOnBoardingCheckingProcessingService,
            CIMBCustomerLoanStatusService cimbCustomerLoanStatusService
            )
        {
            _logger = logger;
            _cimbDataProcessingService = cimbDataProcessingService;
            _cimbService = cimbService;
            _cimbOnBoardingCheckingProcessingService = cimbOnBoardingCheckingProcessingService;
            _cimbCustomerLoanStatusService = cimbCustomerLoanStatusService;
        }

        [AllowAnonymous]
        [HttpPost("loan/check-onboarding-permission")]
        public async Task<ActionResult> CanOnBoarding(string customerId)
        {
            try
            {
                var response = await _cimbOnBoardingCheckingProcessingService.CanOnBoardingCIMBSystem(customerId);

                if (response.SystemCode == CIMBSystemCode.BAD_REQUEST.ToString())
                {
                    return BadRequest(ResponseContext.GetErrorInstance(response.Message, response));
                }

                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("loan-ms/loan-onboarding/submit")]
        public async Task<ActionResult> SubmitCustomerLoan()
        {
            try
            {
                await _cimbService.SyncCIMBDataProcessing();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("loan-ms/loan-onboarding/sync-customer-loan-status")]
        public async Task<ActionResult> SyncCustomerLoanStatus()
        {
            try
            {
                var result = await _cimbCustomerLoanStatusService.SyncCustomerLoanStatus();

                if (result.SystemCode == CIMBSystemCode.BAD_REQUEST.ToString())
                {
                    return BadRequest(ResponseContext.GetErrorInstance(result.Message, result));
                }

                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("loan/status")]
        public async Task<ActionResult> GetLoanStatus(CIMBLoanStatusDto request)
        {
            try
            {
                var result = await _cimbCustomerLoanStatusService.GetLoanStatus(request);

                if (result.SystemCode == CIMBSystemCode.BAD_REQUEST.ToString())
                {
                    return BadRequest(ResponseContext.GetErrorInstance(result.Message, result));
                }

                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpGet("{customerId}/get-payload")]
        public async Task<ActionResult<ResponseContext>> GetPayloadAsync(string customerId)
        {
            try
            {
                var result = await _cimbDataProcessingService.GetPayloadAsync(customerId);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                    data = null
                });
            }
        }
    
        [HttpPost("{id}/create")]
        public async Task<ActionResult<ResponseContext>> CreateAsync(string id)
        {
            try
            {
                await _cimbDataProcessingService.CreateAsync(id);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }
    }
}
