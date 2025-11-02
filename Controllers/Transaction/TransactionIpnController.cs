using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.Transaction;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.Transaction
{
    [Route("api/transaction-ipn")]
    public class TransactionIpnController : BaseController
    {
        private readonly ILogger<TransactionIpnController> _logger;
        private readonly ITransactionIpnService _transactionIpnService;
        

        public TransactionIpnController(
            ILogger<TransactionIpnController> logger,
            ITransactionIpnService transactionIpnService
        )
        {
            _logger = logger;
            _transactionIpnService = transactionIpnService;
        }

        [HttpGet]
        [Authorize(Roles = PermissionCost.AdminPermission.Admin_Transaction_Ipn_ViewAll)]
        public async Task<ActionResult> Get([FromQuery] TransactionIpnRequest request)
        {
            try
            {
                var transactions = await _transactionIpnService.GetAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(transactions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
