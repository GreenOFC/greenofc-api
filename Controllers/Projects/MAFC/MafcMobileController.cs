using _24hplusdotnetcore.ModelDtos.MAFC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.MAFC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace _24hplusdotnetcore.Controllers.MAFC
{
    [Route("api/mobile/mafc")]
    public class MafcMobileController: BaseController
    {
        private readonly ILogger<MafcMobileController> _logger;
        private readonly IMafcService _mafcService;

        public MafcMobileController(
            ILogger<MafcMobileController> logger,
            IMafcService mafcService)
        {
            _logger = logger;
            _mafcService = mafcService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateMafcStep1Request createMafcRequest)
        {
            try
            {
                var response = await _mafcService.CreateAsync(createMafcRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
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

        [HttpPut("{id}/step-1")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep1Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep1Async(id, updateMafcRequest);
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

        [HttpPut("{id}/step-2")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep2Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep2Async(id, updateMafcRequest);
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

        [HttpPut("{id}/step-3")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep3Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep3Async(id, updateMafcRequest);
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

        [HttpPut("{id}/step-4")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep4Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep4Async(id, updateMafcRequest);
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

        [HttpPut("{id}/step-5")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep5Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep5Async(id, updateMafcRequest);
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

        [HttpPut("{id}/step-6")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep6Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep6Async(id, updateMafcRequest);
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

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetMafcResquest getMafcResquest)
        {
            try
            {
                var result = await _mafcService.GetAsync(getMafcResquest);
                return Ok(ResponseContext.GetSuccessInstance(result));
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
                var result = await _mafcService.GetDetailAsync(id);
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
