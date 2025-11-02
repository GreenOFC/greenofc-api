using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Vps;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/lead-vps")]
    public class LeadVpsController : BaseController
    {
        private readonly ILogger<LeadVpsController> _logger;
        private readonly ILeadVpsService _leadVpsService;

        public LeadVpsController(ILogger<LeadVpsController> logger, ILeadVpsService leadVpsService)
        {
            _logger = logger;
            _leadVpsService = leadVpsService;
        }

        [CheckUserApprovedAuthotization]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLeadVpsDto request)
        {
            try
            {
                await _leadVpsService.CreateAsync(request);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetLeadVpsList([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var leadVibResponse = await _leadVpsService.GetList(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(leadVibResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                var leadVibResponse = await _leadVpsService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(leadVibResponse));
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
                await _leadVpsService.DeleteAsync(id);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadVpsDto request)
        {
            try
            {
                await _leadVpsService.UpdateAsync(id, request);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
