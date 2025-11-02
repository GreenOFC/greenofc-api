using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _24hplusdotnetcore.ModelDtos.History;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/history")]
    public class HistoryController : BaseController
    {
        private readonly ILogger<HistoryController> _logger;
        private readonly IHistoryService _historyService;
        public HistoryController(
            ILogger<HistoryController> logger,
            IHistoryService historyService
            )
        {
            _logger = logger;
            _historyService = historyService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetDetailAsync([FromQuery] GetHistoryRequest request)
        {
            try
            {
                var result = await _historyService.GetAsync(request);
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