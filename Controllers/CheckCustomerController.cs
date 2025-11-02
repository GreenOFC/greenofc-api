using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
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
    [Route("api/check-customers")]
    public class CheckCustomerController : BaseController
    {
        private readonly ILogger<CheckCustomerController> _logger;
        private readonly ICheckCustomerService _checkCustomerService;

        public CheckCustomerController(
            ILogger<CheckCustomerController> logger,
            ICheckCustomerService checkCustomerService)
        {
            _logger = logger;
            _checkCustomerService = checkCustomerService;
        }

        [HttpPost]
        public async Task<IActionResult> CheckCustomerAsync([FromForm] IFormFile file)
        {
            try
            {
                await _checkCustomerService.CheckAsync(file);

                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var result = await _checkCustomerService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetailAsync(string id, [FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var result = await _checkCustomerService.GetDetailAsync(id, pagingRequest);
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
