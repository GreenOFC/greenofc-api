using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.TransactionHistories;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/transaction-histories")]
    public class TransactionHistoryController : BaseController
    {
        private readonly ILogger<TransactionHistoryController> _logger;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public TransactionHistoryController(
            ILogger<TransactionHistoryController> logger, 
            ITransactionHistoryService transactionHistoryService)
        {
            _logger = logger;
            _transactionHistoryService = transactionHistoryService;
        }

        [Authorize(Roles = PermissionCost.TransactionPermission.TransactionViewHistory)]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] TransactionHistoryRequest pagingRequest)
        {
            try
            {
                var transactionHistories = await _transactionHistoryService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(transactionHistories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
