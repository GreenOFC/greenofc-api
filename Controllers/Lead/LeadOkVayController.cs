using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.LeadOkVays;
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
    [Route("api/lead-ok-vays")]
    public class LeadOkVayController: BaseController
    {
        private readonly ILogger<LeadOkVayController> _logger;
        private readonly ILeadOkVayService _leadOkVayService;

        public LeadOkVayController(
            ILogger<LeadOkVayController> logger,
            ILeadOkVayService leadOkVayService)
        {
            _logger = logger;
            _leadOkVayService = leadOkVayService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetLeadOkVayRequest getLeadOkVayRequest)
        {
            try
            {
                var leadOkVayResponse = await _leadOkVayService.GetAsync(getLeadOkVayRequest);
                return Ok(ResponseContext.GetSuccessInstance(leadOkVayResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLeadOkVayRequest createLeadOkVayRequest)
        {
            try
            {
                await _leadOkVayService.CreateAsync(createLeadOkVayRequest);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadOkVayRequest updateLeadOkVayRequest)
        {
            try
            {
                await _leadOkVayService.UpdateAsync(id, updateLeadOkVayRequest);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var leadOkVay = await _leadOkVayService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(leadOkVay));
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _leadOkVayService.DeleteAsync(id);
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

        [Authorize(Roles = PermissionCost.LeadOkVayApprove)]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> MarkApproveAsync(string id)
        {
            try
            {
                await _leadOkVayService.MarkApproveAsync(id);
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

        [Authorize(Roles = PermissionCost.LeadOkVayReject)]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> MarkRejectAsync(string id, RejectLeadOkVayRequest rejectLeadOkVayRequest)
        {
            try
            {
                await _leadOkVayService.MarkRejectAsync(id, rejectLeadOkVayRequest.Reason);
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
