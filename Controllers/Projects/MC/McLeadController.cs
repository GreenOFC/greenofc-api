using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.MC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace _24hplusdotnetcore.Controllers.MC
{
    public class McLeadController : BaseController
    {
        private readonly ILogger<McLeadController> _logger;
        private readonly IMcApplicationService _mcApplicationService;

        public McLeadController(
            ILogger<McLeadController> logger,
            IMcApplicationService mcApplicationService)
        {
            _logger = logger;
            _mcApplicationService = mcApplicationService;
        }

        [CheckUserApprovedAuthotization]
        [TypeFilter(typeof(LeadEcAuthorizeAttribute), Arguments = new object[] { LeadSourceType.MC })]
        [HttpPost("api/mc-lead-step-1")]
        public async Task<IActionResult> CreateAsync(CreateMcLeadStep1Request createMcRequest)
        {
            try
            {
                var response = await _mcApplicationService.CreateStep1Async(createMcRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
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

        [HttpPut("api/mc-lead/{id}/step-1")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcLeadStep1Request updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStep1Async(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
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

        [HttpPut("api/mc-lead/{id}/step-2")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcLeadStep2Request updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStep2Async(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
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

    }
}
