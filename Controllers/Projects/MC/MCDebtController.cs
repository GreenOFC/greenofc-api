using _24hplusdotnetcore.ModelDtos.MCDebts;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.MC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/mc-debts")]
    public class MCDebtController: BaseController
    {
        private readonly ILogger<MCDebtController> _logger;
        private readonly IMCDebtService _mCDebtService;

        public MCDebtController(
            ILogger<MCDebtController> logger,
            IMCDebtService mCDebtService)
        {
            _logger = logger;
            _mCDebtService = mCDebtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetMCDebtRequest getMCDebtRequest)
        {
            try
            {
                var mcDebts = await _mCDebtService.GetAsync(getMCDebtRequest);
                return Ok(ResponseContext.GetSuccessInstance(mcDebts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateMCDebtRequest createMCDebtRequest)
        {
            try
            {
                await _mCDebtService.CreateAsync(createMCDebtRequest.AppNumber);
                return Ok(ResponseContext.GetSuccessInstance());
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var mcDebt = await _mCDebtService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(mcDebt));
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

        [HttpPut("{id}/payment-confirm")]
        public async Task<IActionResult> ConfirmPaymentAsync(string id)
        {
            try
            {
                await _mCDebtService.ConfirmPaymentAsync(id);
                return Ok(ResponseContext.GetSuccessInstance());
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

        [HttpPut("{id}/unfollow")]
        public async Task<IActionResult> UnFollowAsync(string id)
        {
            try
            {
                await _mCDebtService.UnFollowAsync(id);
                return Ok(ResponseContext.GetSuccessInstance());
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
