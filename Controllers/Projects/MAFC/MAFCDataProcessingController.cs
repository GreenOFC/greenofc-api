using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.MAFC;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.MAFC
{
    [Route("api/mafc-data-processings")]
    public class MAFCDataProcessingController : BaseController
    {
        private readonly ILogger<MAFCDataProcessingController> _logger;
        private readonly DataMAFCProcessingServices _dataMAFCProcessingServices;
        private readonly IMAFCDataEntryService _mafcDataEntryService;

        public MAFCDataProcessingController(
            ILogger<MAFCDataProcessingController> logger,
            IMAFCDataEntryService mafcDataEntryService,
            DataMAFCProcessingServices dataMAFCProcessingServices)
        {
            _logger = logger;
            _dataMAFCProcessingServices = dataMAFCProcessingServices;
            _mafcDataEntryService = mafcDataEntryService;
        }

        [Authorize(Roles = PermissionCost.MafcGetPayload)]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetDataMAFCProcessingRequest getDataMAFCProcessingRequest)
        {
            try
            {
                var result = await _dataMAFCProcessingServices.GetAsync(getDataMAFCProcessingRequest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateDataMAFCProcessingRequest updateDataMAFCProcessingRequest)
        {
            try
            {
                await _dataMAFCProcessingServices.UpdateAsync(id, updateDataMAFCProcessingRequest);
                if (updateDataMAFCProcessingRequest.Status == DataProcessingStatus.IN_PROGRESS)
                {
                    BackgroundJob.Enqueue<IMAFCDataEntryService>(x => x.ProcessRecordAsync(id));
                }
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
