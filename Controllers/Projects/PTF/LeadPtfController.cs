using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.LeadPtf;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/lead-ptf")]
    public class LeadPtfController: BaseController
    {
        private readonly ILogger<LeadPtfController> _logger;
        private readonly ILeadPtfService _leadPtfService;

        public LeadPtfController(
            ILogger<LeadPtfController> logger, 
            ILeadPtfService leadPtfService)
        {
            _logger = logger;
            _leadPtfService = leadPtfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetLeadPtfRequest pagingRequest)
        {
            try
            {
                var result = await _leadPtfService.GetAsync(pagingRequest);
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
                var result = await _leadPtfService.GetDetailAsync(id);
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

        [CheckUserApprovedAuthotization]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLeadPtfRequest createLeadPtfRequest)
        {
            try
            {
                var response = await _leadPtfService.CreateAsync(createLeadPtfRequest);
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

        [CheckUserApprovedAuthotization]
        [HttpPost("step-1")]
        public async Task<IActionResult> CreateAsync(CreateLeadPtfStep1Request createLeadPtfStep1Request)
        {
            try
            {
                var response = await _leadPtfService.CreateAsync(createLeadPtfStep1Request);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadPtfRequest updateLeadPtfRequest)
        {
            try
            {
                await _leadPtfService.UpdateAsync(id, updateLeadPtfRequest);
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

        [HttpPut("{id}/step-1")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadPtfStep1Request updateLeadPtfStep1Request)
        {
            try
            {
                await _leadPtfService.UpdateAsync(id, updateLeadPtfStep1Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadPtfStep2Request updateLeadPtfStep2Request)
        {
            try
            {
                await _leadPtfService.UpdateAsync(id, updateLeadPtfStep2Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadPtfStep3Request updateLeadPtfStep3Request)
        {
            try
            {
                await _leadPtfService.UpdateAsync(id, updateLeadPtfStep3Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadPtfStep4Request updateLeadPtfStep4Request)
        {
            try
            {
                await _leadPtfService.UpdateAsync(id, updateLeadPtfStep4Request);
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
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadPtfStep5Request updateLeadPtfStep5Request)
        {
            try
            {
                await _leadPtfService.UpdateAsync(id, updateLeadPtfStep5Request);
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

        [HttpPut("{id}/documents")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateDocumentLeadPtfRequest updateDocumentLeadPtfRequest)
        {
            try
            {
                await _leadPtfService.UpdateAsync(id, updateDocumentLeadPtfRequest);
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

        [HttpGet("category")]
        public async Task<IActionResult> GetCategoryAsync(string productLine)
        {
            try
            {
                var result = await _leadPtfService.GetCategoryAsync(productLine);
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
