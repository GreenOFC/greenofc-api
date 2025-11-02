using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.MAFC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.MAFC
{
    [Route("api/mafc-s37")]
    public class MAFCS37Controller: BaseController
    {
        private readonly ILogger<MAFCS37Controller> _logger;
        private readonly IMAFCS37Service _iMAFCS37Service;

        public MAFCS37Controller(
            ILogger<MAFCS37Controller> logger,
            IMAFCS37Service iMAFCS37Service)
        {
            _logger = logger;
            _iMAFCS37Service = iMAFCS37Service;
        }

        [AllowAnonymous]
        [HttpPost("submit")]
        public async Task<ActionResult<ResponseContext>> SubmitAsync(MAFCSubmitS37Request mAFCSubmitS37Request)
        {
            try
            {
                var result = await _iMAFCS37Service.SubmitAsync(mAFCSubmitS37Request);
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

        [AllowAnonymous]
        [HttpPost("polling")]
        public async Task<ActionResult<ResponseContext>> PollingAsync(MAFCPollingS37Request mAFCPollingS37Request)
        {
            try
            {
                var result = await _iMAFCS37Service.PollingAsync(mAFCPollingS37Request);
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
    }
}
