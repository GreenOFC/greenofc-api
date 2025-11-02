using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.UserHistories;
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
    [Route("api/user-histories")]
    public class UserHistoryController : BaseController
    {
        private readonly ILogger<UserHistoryController> _logger;
        private readonly IUserHistoryService _userHistoryService;

        public UserHistoryController(
            ILogger<UserHistoryController> logger,
            IUserHistoryService userHistoryService)
        {
            _logger = logger;
            _userHistoryService = userHistoryService;
        }

        [Authorize(Roles = PermissionCost.UserViewHistory)]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] UserHistoryRequest pagingRequest)
        {
            try
            {
                var userHistories = await _userHistoryService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(userHistories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
