using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.MC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [ApiController]
    [Route("api/log")]
    public class HistoryCallApiLogController : BaseController
    {
        private readonly ILogger<HistoryCallApiLogController> _logger;
        private readonly IHistoryCallApiLogService _historyCallApiLogService;

        public HistoryCallApiLogController(
            ILogger<HistoryCallApiLogController> logger,
            IHistoryCallApiLogService historyCallApiLogService)
        {
            _logger = logger;
            _historyCallApiLogService = historyCallApiLogService;
        }


        [HttpGet("history-api")]
        public async Task<IActionResult> GetAsync([FromQuery] HistoryCallApiLogRequest pagingRequest)
        {
            try
            {
                var result = await _historyCallApiLogService.GetAsync(pagingRequest);
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
