using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.LeadEcs;
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
    [Route("api/lead-ecs")]
    public class LeadEcController: BaseController
    {
        private readonly ILogger<LeadEcController> _logger;
        private readonly ILeadEcService _leadEcService;

        public LeadEcController(ILogger<LeadEcController> logger, ILeadEcService leadEcService)
        {
            _logger = logger;
            _leadEcService = leadEcService;
        }

        [CheckUserApprovedAuthotization]
        [TypeFilter(typeof(LeadEcAuthorizeAttribute), Arguments = new object[] { LeadSourceType.EC })]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLeadEcRequest createLeadEcRequest)
        {
            try
            {
                var response = await _leadEcService.CreateAsync(createLeadEcRequest);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep1Request updateLeadEcStep1Request)
        {
            try
            {
                await _leadEcService.UpdateStep1Async(id, updateLeadEcStep1Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep2Request updateLeadEcStep2Request)
        {
            try
            {
                await _leadEcService.UpdateStep2Async(id, updateLeadEcStep2Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep3Request updateLeadEcStep3Request)
        {
            try
            {
                await _leadEcService.UpdateStep3Async(id, updateLeadEcStep3Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep4Request updateLeadEcStep4Request)
        {
            try
            {
                await _leadEcService.UpdateStep4Async(id, updateLeadEcStep4Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep5Request updateLeadEcStep5Request)
        {
            try
            {
                await _leadEcService.UpdateStep5Async(id, updateLeadEcStep5Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep6Request updateLeadEcStep6Request)
        {
            try
            {
                await _leadEcService.UpdateStep6Async(id, updateLeadEcStep6Request);
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

        [HttpPut("{id}/step-7")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep7Request updateLeadEcStep7Request)
        {
            try
            {
                await _leadEcService.UpdateStep7Async(id, updateLeadEcStep7Request);
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

        [HttpPut("{id}/step-8")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadEcStep8Request updateLeadEcStep8Request)
        {
            try
            {
                await _leadEcService.UpdateStep8Async(id, updateLeadEcStep8Request);
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
                var result = await _leadEcService.GetDetailAsync(id);
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

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetLeadEcRequest getLeadEcRequest)
        {
            try
            {
                var result = await _leadEcService.GetAsync(getLeadEcRequest);
                return Ok(ResponseContext.GetSuccessInstance(result));
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
                var resources = await _leadEcService.GetResourceAsync<GetLeadEcCityResponse>(LeadEcResourceType.CITY);
                return Ok(ResponseContext.GetSuccessInstance(resources));
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
                var resources = await _leadEcService.GetResourceAsync<GetLeadEcDistrictResponse>(LeadEcResourceType.DISTRICT, stateId);
                return Ok(ResponseContext.GetSuccessInstance(resources));
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
                var resources = await _leadEcService.GetResourceAsync<GetLeadEcWardResponse>(LeadEcResourceType.WARD, cityId);
                return Ok(ResponseContext.GetSuccessInstance(resources));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("issue-places")]
        public async Task<IActionResult> GetIssuePlacedAsync()
        {
            try
            {
                var resources = await _leadEcService.GetResourceAsync<GetLeadEcBankResponse>(LeadEcResourceType.ISSUE_PLACE);
                return Ok(ResponseContext.GetSuccessInstance(resources));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("banks")]
        public async Task<IActionResult> GetBankAsync()
        {
            try
            {
                var resources = await _leadEcService.GetResourceAsync<GetLeadEcBankResponse>(LeadEcResourceType.BANK);
                return Ok(ResponseContext.GetSuccessInstance(resources));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("bank-branches")]
        public async Task<IActionResult> GetBankBranchAsync(string parentCode)
        {
            try
            {
                var resources = await _leadEcService.GetResourceAsync<GetLeadEcBankBranchResponse>(LeadEcResourceType.BANK_BRANCH, parentCode);
                return Ok(ResponseContext.GetSuccessInstance(resources));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProductAsync(string employmentStatusId)
        {
            try
            {
                var resources = await _leadEcService.GetProductAsync(employmentStatusId);
                return Ok(ResponseContext.GetSuccessInstance(resources));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}/offers")]
        public async Task<IActionResult> GetOfferAsync(string id)
        {
            try
            {
                var offers = await _leadEcService.GetOfferAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(offers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.EcUpdateRecord)]
        [HttpPut("{id}/record-files")]
        public async Task<IActionResult> UpdateRecordFileAsync(string id, UpdateEcRecordFileRequest updateEcRecordFileRequest)
        {
            try
            {
                await _leadEcService.UpdateRecordFileAsync(id, updateEcRecordFileRequest);
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

        [Authorize(Roles = PermissionCost.EcUpdateStatus)]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateRecordFileAsync(string id, UpdateEcStatusRequest updateEcStatusRequest)
        {
            try
            {
                await _leadEcService.UpdateStatusAsync(id, updateEcStatusRequest);
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
