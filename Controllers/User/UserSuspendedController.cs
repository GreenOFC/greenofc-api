using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.UserSuspendeds;
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
    [Route("api/user-suspended")]
    public class UserSuspendedController: BaseController
    {
        private readonly ILogger<UserSuspendedController> _logger;
        private readonly IUserSuspendedService _userSuspendedService;

        public UserSuspendedController(
            ILogger<UserSuspendedController> logger,
            IUserSuspendedService leadOkVayService)
        {
            _logger = logger;
            _userSuspendedService = leadOkVayService;
        }

        [Authorize(Roles = PermissionCost.GetListSuspendedUserProfile)]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var userSuspendedResponse = await _userSuspendedService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(userSuspendedResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserSuspended createLeadOkVayRequest)
        {
            try
            {
                await _userSuspendedService.CreateAsync(createLeadOkVayRequest);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateUserSuspended updateLeadOkVayRequest)
        {
            try
            {
                await _userSuspendedService.UpdateAsync(id, updateLeadOkVayRequest);
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
                var leadOkVay = await _userSuspendedService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(leadOkVay));
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
                await _userSuspendedService.DeleteAsync(id);
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

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetAsync(string id, [FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var userSuspendedResponse = await _userSuspendedService.GetUserInUserSuspendedAsync(id, pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(userSuspendedResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{id}/users/{userId}")]
        public async Task<IActionResult> CreateAsync(string id, string userId)
        {
            try
            {
                await _userSuspendedService.CreateAsync(id, userId);
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

        [HttpPost("{id}/users")]
        public async Task<IActionResult> CreateAsync(string id, UpdateUserInUserSuspendedRequest updateUserInUserSuspendedRequest)
        {
            try
            {
                await _userSuspendedService.CreateAsync(id, updateUserInUserSuspendedRequest.UserNames);
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

        [HttpDelete("{id}/users/{userId}")]
        public async Task<IActionResult> DeleteAsync(string id, string userId)
        {
            try
            {
                await _userSuspendedService.DeleteAsync(id, userId);
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
