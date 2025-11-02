using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.MC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.MC
{
    [Route("api/mc-data-processings")]
    public class MCDataProcessingController: BaseController
    {
        private readonly ILogger<MCDataProcessingController> _logger;
        private readonly DataMCProcessingServices _dataMCProcessingServices;

        public MCDataProcessingController(
            ILogger<MCDataProcessingController> logger,
            DataMCProcessingServices dataMCProcessingServices)
        {
            _logger = logger;
            _dataMCProcessingServices = dataMCProcessingServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetDataMCProcessingRequest getDataMCProcessingRequest)
        {
            try
            {
                var result = await _dataMCProcessingServices.GetAsync(getDataMCProcessingRequest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.DataMCProsessingCreate)]
        [HttpPost("{customerId}/resend")]
        public async Task<IActionResult> Resend(string customerId)
        {
            try
            {
                var result = await _dataMCProcessingServices.Resend(customerId);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = result
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
