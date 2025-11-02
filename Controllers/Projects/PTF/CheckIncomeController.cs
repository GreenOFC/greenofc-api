using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.PtfOmnis.CheckIncomeDto;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.PtfOmnis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/ptf-check-income")]
    public class CheckIncomeController: BaseController
    {
        private readonly ILogger<CheckIncomeController> _logger;
        private readonly IPtfCheckIncomeService _ptfCheckIncomeService;

        public CheckIncomeController(
            ILogger<CheckIncomeController> logger, 
            IPtfCheckIncomeService ptfCheckIncomeService)
        {
            _logger = logger;
            _ptfCheckIncomeService = ptfCheckIncomeService;
        }

        // [HttpGet]
        // [Authorize(Roles = PermissionCost.CheckSimGet)]
        // public async Task<IActionResult> GetAsync([FromQuery] GetCheckSimRequest pagingRequest)
        // {
        //     try
        //     {
        //         var checkSims = await _checkSimService.GetAsync(pagingRequest);
        //         return Ok(ResponseContext.GetSuccessInstance(checkSims));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, ex.Message);
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }

        // [HttpGet("export")]
        // [Authorize(Roles = PermissionCost.CheckSimExport)]
        // public async Task<IActionResult> ExportAsync([FromQuery] GetCheckSimRequest pagingRequest)
        // {
        //     try
        //     {
        //         var checkSims = await _checkSimService.GetAsync(pagingRequest);
        //         return Ok(ResponseContext.GetSuccessInstance(checkSims));
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, ex.Message);
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }

        [HttpPost("query")]
        public async Task<IActionResult> QueryAsync(PtfQueryRequestDto request)
        {
            try
            {
                var result = await _ptfCheckIncomeService.QueryAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtpAsync(PtfQueryRequestDto request)
        {
            try
            {
                var result = await _ptfCheckIncomeService.SendOtpAsync(request);
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

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtpAsync(PtfVerifyOtpRequestDto request)
        {
            try
            {
                var result = await _ptfCheckIncomeService.VerifyOtpAsync(request);
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
