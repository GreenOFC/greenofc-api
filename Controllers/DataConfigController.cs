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
    [Route("api/data-configs")]
    public class DataConfigController: BaseController
    {
        private readonly ILogger<DataConfigController> _logger;
        private readonly IDataConfigService _dataConfigService;

        public DataConfigController(ILogger<DataConfigController> logger, IDataConfigService dataConfigService)
        {
            _logger = logger;
            _dataConfigService = dataConfigService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAsync(string greenType, string type)
        {
            try
            {
                var dataConfigs = await _dataConfigService.GetAsync(greenType, type);
                return Ok(ResponseContext.GetSuccessInstance(dataConfigs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
