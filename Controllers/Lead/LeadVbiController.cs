using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadVbis;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/lead-vbis")]
    public class LeadVbiController: BaseController
    {
        private readonly ILogger<LeadVbiController> _logger;
        private readonly ILeadVbiService _leadVbiService;

        public LeadVbiController(
            ILogger<LeadVbiController> logger,
            ILeadVbiService leadVbiService)
        {
            _logger = logger;
            _leadVbiService = leadVbiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var leadVbiResponse = await _leadVbiService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(leadVbiResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [CheckUserApprovedAuthotization]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLeadVbiRequest createLeadVbiRequest)
        {
            try
            {
                await _leadVbiService.CreateAsync(createLeadVbiRequest);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadVbiRequest updateLeadVbiRequest)
        {
            try
            {
                await _leadVbiService.UpdateAsync(id, updateLeadVbiRequest);
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
                var leadVbi = await _leadVbiService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(leadVbi));
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
                await _leadVbiService.DeleteAsync(id);
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
