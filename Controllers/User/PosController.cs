using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace _24hplusdotnetcore.Controllers
{
    [Route("api/pos")]
    public class PosController : BaseController
    {
        private readonly ILogger<PosController> _logger;
        private readonly IPosService _posService;

        public PosController(ILogger<PosController> logger, IPosService posService)
        {
            _logger = logger;
            _posService = posService;
        }


        [Authorize(Roles = PermissionCost.PosCreate)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePosDto request)
        {
            try
            {
                await _posService.CreateAsync(request);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.PosGet)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                var posDetailResponse = await _posService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(posDetailResponse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.PosDelete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _posService.DeleteAsync(id);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.PosUpdate)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdatePosDto request)
        {
            try
            {
                var result = await _posService.UpdateAsync(id, request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.RoleGetBelongUser)]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetPosRequest pagingRequest)
        {
            try
            {
                var result = await _posService.GetListAsync(pagingRequest);

                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{posId}/users")]
        public async Task<IActionResult> GetUserInPos(string posId, [FromQuery] GetPosDto request)
        {
            try
            {
                var result = await _posService.GetUsersInPos(posId, request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPut("{posId}/user/{userId}")]
        public async Task<IActionResult> AddUserToPos(string posId, string userId, [FromBody] IEnumerable<string> roleIds)
        {
            try
            {
                UpdateUserPosDto request = new UpdateUserPosDto
                {
                    UserId = userId,
                    PosId = posId,
                    RoleIds = roleIds
                };

                var addResult = await _posService.AdduserToPos(request);

                if (addResult.IsSuccess)
                {
                    return Ok(addResult);
                }

                return BadRequest(addResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpDelete("{posId}/user/{userId}")]
        public async Task<IActionResult> RemoveUserFromPos(string posId, string userId)
        {
            try
            {
                RemoveUserFromPosDto request = new RemoveUserFromPosDto
                {
                    UserId = userId,
                    PosId = posId
                };

                var addResult = await _posService.RemoveUserFromPos(request);

                if (addResult.IsSuccess)
                {
                    return Ok(addResult);
                }

                return BadRequest(addResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPost("{id}/managerment")]
        public async Task<IActionResult> AssignManagerAsync(string id, AssignPosManagerDto request)
        {
            try
            {
                await _posService.AssignManagerAsync(id, request);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
