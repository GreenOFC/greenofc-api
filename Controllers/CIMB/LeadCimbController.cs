using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.LeadCimbs;
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
    [Route("api/lead-cimbs")]
    public class LeadCimbController: BaseController
    {
        private readonly ILogger<LeadCimbController> _logger;
        private readonly ILeadCimbService _leadCimbService;
        private readonly ILeadCimbResourceService _leadCimbResourceService;

        public LeadCimbController(
            ILogger<LeadCimbController> logger,
            ILeadCimbService leadCimbService,
            ILeadCimbResourceService leadCimbResourceService)
        {
            _logger = logger;
            _leadCimbService = leadCimbService;
            _leadCimbResourceService = leadCimbResourceService;
        }

        [HttpPost]
        public IActionResult Create(CreateLeadCimbRequest createLeadCimbRequest)
        {
            try
            {
                return Ok(ResponseContext.GetErrorInstance("Thông báo Dự án tạm thời đang tạm ngưng. Anh/chị tạm thời chuyển sang Dự án khác"));
                // var response = await _leadCimbService.CreateAsync(createLeadCimbRequest);
                // return Ok(ResponseContext.GetSuccessInstance(response));
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

        [CheckUserApprovedAuthotization]
        [HttpPost("{id}/send-verify")]
        public async Task<IActionResult> SendVerifyAsync(string id, CimbSendVerifyRequest cimbSendVerifyRequest)
        {
            try
            {
                var response = await _leadCimbService.SendVerifyAsync(id, cimbSendVerifyRequest);
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

        [HttpPost("{id}/verify")]
        public async Task<IActionResult> VerifyAsync(string id, CimbVerifyRequest cimbVerifyRequest)
        {
            try
            {
                var response = await _leadCimbService.VerifyAsync(id, cimbVerifyRequest);
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
        public IActionResult Update(string id, UpdateLeadCimbStep1Request updateLeadCimbStep1Request)
        {
            try
            {
                return Ok(ResponseContext.GetErrorInstance("Thông báo Dự án tạm thời đang tạm ngưng. Anh/chị tạm thời chuyển sang Dự án khác"));
                // var response = await _leadCimbService.UpdateStep1Async(id, updateLeadCimbStep1Request);
                // return Ok(ResponseContext.GetSuccessInstance(response));
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadCimbStep2Request updateLeadCimbStep2Request)
        {
            try
            {
                await _leadCimbService.UpdateStep2Async(id, updateLeadCimbStep2Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadCimbStep3Request updateLeadCimbStep3Request)
        {
            try
            {
                await _leadCimbService.UpdateStep3Async(id, updateLeadCimbStep3Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadCimbStep4Request updateLeadCimbStep4Request)
        {
            try
            {
                await _leadCimbService.UpdateStep4Async(id, updateLeadCimbStep4Request);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var leadCimb = await _leadCimbService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(leadCimb));
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _leadCimbService.DeleteAsync(id);
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
        public async Task<IActionResult> GetAsync([FromQuery] GetLeadCimbRequest getLeadCimbRequest)
        {
            try
            {
                var cimbs = await _leadCimbService.GetAsync(getLeadCimbRequest);
                return Ok(ResponseContext.GetSuccessInstance(cimbs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/submit")]
        public async Task<IActionResult> SubmitAsync(string id)
        {
            try
            {
                await _leadCimbService.SubmitAsync(id);
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

        [HttpGet("cities")]
        public async Task<IActionResult> GetCityAsync()
        {
            try
            {
                var cities = await _leadCimbService.GetCitiesAsync();
                return Ok(ResponseContext.GetSuccessInstance(cities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("districts")]
        public async Task<IActionResult> GetDistrictAsync(string stateId)
        {
            try
            {
                var districts = await _leadCimbService.GetDistrictAsync(stateId);
                return Ok(ResponseContext.GetSuccessInstance(districts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("wards")]
        public async Task<IActionResult> GetWardAsync(string cityId)
        {
            try
            {
                var wards = await _leadCimbService.GetWardAsync(cityId);
                return Ok(ResponseContext.GetSuccessInstance(wards));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("master-data")]
        public async Task<IActionResult> GetMasterData()
        {
            try
            {
                await _leadCimbResourceService.SyncAsync();
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpGet("loan-informations")]
        public async Task<IActionResult> GetLoanInfomationAsync([FromQuery] LeadCimbLoanInfomationRequest request)
        {
            try
            {
                var result = await _leadCimbService.GetLoanInfomationAsync(request);
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
