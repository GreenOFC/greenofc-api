using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.Roles;
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
    [Route("api/roles")]
    public class RolesController : BaseController
    {
        private ILogger<RolesController> _logger;
        private readonly IRoleService _roleService;
        private readonly IUserServices _userServices;

        public RolesController(
            IRoleService roleServices, 
            ILogger<RolesController> logger,
            IUserServices userServices)
        {
            _roleService = roleServices;
            _logger = logger;
            _userServices = userServices;
        }

        [Authorize(Roles = PermissionCost.RoleGetList)]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetRoleRequest getRoleRequest)
        {
            try
            {
                var roles = await _roleService.GetAsync(getRoleRequest);
                return Ok(ResponseContext.GetSuccessInstance(roles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.RoleCreate)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRoleRequest createRoleRequest)
        {
            try
            {
                await _roleService.CreateAsync(createRoleRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.RoleUpdate)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateRoleRequest updateRoleRequest)
        {
            try
            {
                await _roleService.UpdateAsync(id, updateRoleRequest);
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

        [Authorize(Roles = PermissionCost.RoleGet)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var role = await _roleService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(role));
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

        [Authorize(Roles = PermissionCost.RoleDelete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _roleService.DeleteAsync(id);
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

        [Authorize(Roles = PermissionCost.RoleGetBelongUser)]
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetAsync(string id, [FromQuery] GetUserRoleRequest getUserRoleRequest)
        {
            try
            {
                var roles = await _userServices.GetAsync(id, getUserRoleRequest);
                return Ok(ResponseContext.GetSuccessInstance(roles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.AddUserRole)]
        [HttpPost("{roleId}/users/{userId}")]
        public async Task<IActionResult> AddUserAsync(string roleId, string userId)
        {
            try
            {
                await _userServices.AddRoleAsync(userId, roleId);
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

        [Authorize(Roles = PermissionCost.RemoveUserRole)]
        [HttpDelete("{roleId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserAsync(string roleId, string userId)
        {
            try
            {
                await _userServices.RemoveRoleAsync(userId, roleId);
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