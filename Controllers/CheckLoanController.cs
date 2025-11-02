using _24hplusdotnetcore.ModelDtos.CheckLoans;
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
    [ApiController]
    [Route("api/tool/check-loan")]
    public class CheckLoanController : BaseController
    {
        private readonly ILogger<CheckLoanController> _logger;
        private readonly ICheckLoanService _checkLoanService;

        public CheckLoanController(
            ILogger<CheckLoanController> logger,
            ICheckLoanService checkLoanService)
        {
            _logger = logger;
            _checkLoanService = checkLoanService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] CheckLoanRequest checkLoanRequest)
        {
            try
            {
                var result = await _checkLoanService.CheckLoanAsync(checkLoanRequest);
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
