using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.LeadVibs;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/lead-vibs")]
    public class LeadVibsController : BaseController
    {
        private readonly ILogger<LeadVibsController> _logger;
        private readonly ILeadVibService _leadVibService;

        public LeadVibsController(
            ILeadVibService leadVibService,
            ILogger<LeadVibsController> logger)
        {
            _leadVibService = leadVibService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetLeadVibRequest getLeadVibRequest)
        {
            try
            {
                var leadVibResponse = await _leadVibService.GetAsync(getLeadVibRequest);
                return Ok(ResponseContext.GetSuccessInstance(leadVibResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [CheckUserApprovedAuthotization]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLeadVibRequest createLeadVibRequest)
        {
            try
            {
                var existed = await _leadVibService.CheckExistedLeadAsync(createLeadVibRequest.Phone);
                if (existed)
                {
                    return Ok(ResponseContext.GetErrorInstance(string.Format(Common.Message.COMMON_EXISTED, "Lead")));
                }
                await _leadVibService.CreateAsync(createLeadVibRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadVibRequest updateLeadVibRequest)
        {
            try
            {
                await _leadVibService.UpdateAsync(id, updateLeadVibRequest);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var leadVib = await _leadVibService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(leadVib));
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _leadVibService.DeleteAsync(id);
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

        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<ResponseContext>> CountLeadAsync()
        {
            try
            {
                var total = await _leadVibService.CountLeadAsync();
                return Ok(ResponseContext.GetSuccessInstance(total));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
