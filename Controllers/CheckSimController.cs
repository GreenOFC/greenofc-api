using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.CheckSims;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/check-sims")]
    public class CheckSimController: BaseController
    {
        private readonly ILogger<CheckSimController> _logger;
        private readonly ICheckSimService _checkSimService;

        public CheckSimController(
            ILogger<CheckSimController> logger,
            ICheckSimService checkSimService)
        {
            _logger = logger;
            _checkSimService = checkSimService;
        }

        // [Authorize(Roles = PermissionCost.CheckSimMcV2)]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            try
            {
                var result = await _checkSimService.SendOtp(request);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        // [Authorize(Roles = PermissionCost.CheckSimMcV2)]
        [HttpPost("{id}/resend-otp")]
        public async Task<IActionResult> ResendOtp(string id)
        {
            try
            {
                var result = await _checkSimService.ResendOtp(id);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // [Authorize(Roles = PermissionCost.CheckSimMcV2)]
        [HttpPost("send-scoring3p")]
        public async Task<IActionResult> SendScoring3P([FromBody] Scoring3PRequest request)
        {
            try
            {
                var result = await _checkSimService.SendScoring3P(request);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = PermissionCost.CheckSimGet)]
        public async Task<IActionResult> GetAsync([FromQuery] GetCheckSimRequest pagingRequest)
        {
            try
            {
                var checkSims = await _checkSimService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(checkSims));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("export")]
        [Authorize(Roles = PermissionCost.CheckSimExport)]
        public async Task<IActionResult> ExportAsync([FromQuery] GetCheckSimRequest pagingRequest)
        {
            try
            {
                var checkSims = await _checkSimService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(checkSims));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("ptf/check-sim")]
        [Authorize(Roles = PermissionCost.CheckSimPtf)]
        public async Task<IActionResult> CheckSimAsync(PtfScopingCheckSimRequest request)
        {
            try
            {
                var checkSims = await _checkSimService.CheckSimAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(checkSims));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("ptf/send-otp")]
        [Authorize(Roles = PermissionCost.CheckSimPtf)]
        public async Task<IActionResult> SendOtpAsync(PtfScopingSendOtpRequest request)
        {
            try
            {
                var result = await _checkSimService.SendOtpAsync(request);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("ptf/verify-otp")]
        [Authorize(Roles = PermissionCost.CheckSimPtf)]
        public async Task<IActionResult> VerifyOtpAsync(PtfScopingVerifyOtpRequest request)
        {
            try
            {
                var result = await _checkSimService.VerifyOtpAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
