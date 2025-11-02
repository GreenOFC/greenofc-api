using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.eWalletTransaction;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.Transaction;
using _24hplusdotnetcore.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.eWalletTransaction
{
    [Route("api/transaction")]
    public class TransactionController : BaseController
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _eWalletTransactionService;
        private readonly ITransactionLogService _eWalletTransactionLogService;
        private readonly ITransactionIpnService _iTransactionIpnService;
        private readonly IPayMeService _payMeService;
        private readonly PayMeConfig _payMeSetting;
        public TransactionController(
            ILogger<TransactionController> logger,
            ITransactionService ieWalletTransactionService,
            ITransactionLogService ieWalletTransactionLogService,
            ITransactionIpnService iTransactionIpnService,
            IPayMeService payMeService,
            IOptions<PayMeConfig> payMeSetting
        )
        {
            _logger = logger;
            _eWalletTransactionService = ieWalletTransactionService;
            _eWalletTransactionLogService = ieWalletTransactionLogService;
            _iTransactionIpnService = iTransactionIpnService;
            _payMeService = payMeService;
            _payMeSetting = payMeSetting.Value;
        }

        [HttpPost()]
        public async Task<ActionResult> CreateTransactionAsync([FromBody] CreateTransactionDto order)
        {
            try
            {
                var result = await _eWalletTransactionService.CreateTransactionAsync(order);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = PermissionCost.TransactionPermission.Transaction_Refund)]
        [HttpPost("{id}/refund")]
        public async Task<ActionResult> RefundTransactionAsync(string id, [FromBody] RefundTransactionDto dto)
        {
            try
            {
                var result = await _eWalletTransactionService.RefundTransactionAsync(id, dto);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("ipn")]
        public async Task<ActionResult> Ipn(
            [FromHeader(Name = "x-api-client")] string xApiClient,
            [FromHeader(Name = "x-api-key")] string xApiKey,
            [FromHeader(Name = "x-api-action")] string xApiAction,
            [FromHeader(Name = "x-api-validate")] string xApiValidate,
            [FromBody] IpnPaymeBodyRequest Payload)
        {
            try
            {
                var dto = new IpnPaymeEncriptDto()
                {
                    xApiClient = xApiClient,
                    xApiKey = xApiKey,
                    xApiAction = xApiAction,
                    xApiValidate = xApiValidate,
                    xApiMessage = Payload.xApiMessage,
                    Method = "POST",
                };
                var res = await _iTransactionIpnService.CreateAsync(dto);

                HttpContext.Response.Headers.Add("x-api-client", res.xApiClient);
                HttpContext.Response.Headers.Add("x-api-key", res.xApiKey);
                HttpContext.Response.Headers.Add("x-api-action", res.xApiAction);
                HttpContext.Response.Headers.Add("x-api-validate", res.xApiValidate);
                return Ok(new IpnPaymeBodyRequest()
                {
                    xApiMessage = res.xApiMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] TransactionRequest request)
        {
            try
            {
                var transactions = await _eWalletTransactionService.GetTransactions(request, false);
                return Ok(ResponseContext.GetSuccessInstance(transactions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult> GetOwner([FromQuery] TransactionRequest request)
        {
            try
            {
                var transactions = await _eWalletTransactionService.GetTransactions(request, true);
                return Ok(ResponseContext.GetSuccessInstance(transactions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("available")]
        public async Task<ActionResult> GetAvailableTransactionsAsync([FromQuery] TransactionAvailableDto request)
        {
            try
            {
                var transactions = await _eWalletTransactionService.GetAvailableTransactionsAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(transactions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult> GetTransactionByCode(string code)
        {
            try
            {
                var transaction = await _eWalletTransactionService.GetTransactionByCodeAsync(code);
                return Ok(ResponseContext.GetSuccessInstance(transaction));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("log/retrieve")]
        public async Task<ActionResult> GetTransactionLog([FromQuery] string TransactionId)
        {
            try
            {
                var walletLst = string.IsNullOrEmpty(TransactionId) ? _eWalletTransactionLogService.GetListTransaction() : await _eWalletTransactionLogService.GetListTransactionLogById(TransactionId);
                return Ok(walletLst);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
