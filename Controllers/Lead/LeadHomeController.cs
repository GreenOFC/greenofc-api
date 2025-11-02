using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.LeadHomes;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/lead-homes")]
    public class LeadHomeController: BaseController
    {
        private readonly ILogger<LeadHomeController> _logger;
        private readonly ILeadHomeService _leadHomeService;

        public LeadHomeController(
            ILogger<LeadHomeController> logger,
            ILeadHomeService leadHomeService)
        {
            _logger = logger;
            _leadHomeService = leadHomeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetLeadHomeRequest getLeadHomeRequest)
        {
            try
            {
                var leadHomeResponse = await _leadHomeService.GetAsync(getLeadHomeRequest);
                return Ok(ResponseContext.GetSuccessInstance(leadHomeResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [CheckUserApprovedAuthotization]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLeadHomeRequest createLeadHomeRequest)
        {
            try
            {
                await _leadHomeService.CreateAsync(createLeadHomeRequest);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadHomeRequest updateLeadHomeRequest)
        {
            try
            {
                await _leadHomeService.UpdateAsync(id, updateLeadHomeRequest);
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
                var leadHome = await _leadHomeService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(leadHome));
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
                await _leadHomeService.DeleteAsync(id);
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
