using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.F88;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.F88;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/f88s")]
    public class F88Controller: BaseController
    {
        private readonly ILogger<F88Controller> _logger;
        private readonly IF88Service _f88Service;

        public F88Controller(
             ILogger<F88Controller> logger,
             IF88Service f88Service)
        {
            _logger = logger;
            _f88Service = f88Service;
        }

        [CheckUserApprovedAuthotization]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateF88Request createF88Request)
        {
            try
            {
                var f88 = await _f88Service.CreateAsync(createF88Request);

                return Ok(ResponseContext.GetSuccessInstance(f88));
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
        public async Task<IActionResult> GetAsync([FromQuery] GetF88Request getF88Request)
        {
            try
            {
                var f88s = await _f88Service.GetAsync(getF88Request);
                return Ok(ResponseContext.GetSuccessInstance(f88s));
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
                var f88 = await _f88Service.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(f88));
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateF88Request updateF88Request)
        {
            try
            {
                await _f88Service.UpdateAsync(id, updateF88Request);

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

        [HttpGet("{id}/links")]
        public async Task<IActionResult> GetLinkAsync(string id)
        {
            try
            {
                var f88 = await _f88Service.GetLinkAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(f88));
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
