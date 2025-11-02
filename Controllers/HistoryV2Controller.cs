using _24hplusdotnetcore.ModelDtos.History;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/v2/histories")]
    public class HistoryV2Controller: BaseController
    {
        private readonly ILogger<HistoryV2Controller> _logger;
        private readonly IHistoryV2Service _historyV2Service;
        public HistoryV2Controller(
            ILogger<HistoryV2Controller> logger,
            IHistoryV2Service historyV2Service
            )
        {
            _logger = logger;
            _historyV2Service = historyV2Service;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailAsync([FromQuery] GetHistoryRequest request)
        {
            try
            {
                var result = await _historyV2Service.GetAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(result));
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
