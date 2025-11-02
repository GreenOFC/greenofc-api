using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
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
    [Route("api/[controller]")]
    public class GroupNotificationController : BaseController
    {
        private readonly ILogger<GroupNotificationController> _logger;
        private readonly IGroupNotificationService _groupNotificationService;

        public GroupNotificationController(ILogger<GroupNotificationController> logger, IGroupNotificationService groupNotificationService)
        {
            _logger = logger;
            _groupNotificationService = groupNotificationService;
        }

        [Authorize(Roles = PermissionCost.GroupNotiCreate)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupNotificationDto request)
        {
            try
            {
                var result = await _groupNotificationService.Create(request);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg, result));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _groupNotificationService.GetById(id);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg, result));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.GroupNotiDelete)]
        [HttpDelete()]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _groupNotificationService.Delete(id);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg, result));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.GroupNotiUpdate)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateGroupNotificationDto request)
        {
            try
            {
                var result = await _groupNotificationService.Update(id, request);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg, result));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.GroupNotiGet)]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PagingRequest request)
        {
            try
            {
                var result = await _groupNotificationService.GetList(request.TextSearch, request.PageIndex, request.PageSize);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg, result));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
