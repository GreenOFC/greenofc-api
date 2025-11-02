using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.FCM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/[controller]")]
    public class GroupNotificationUserController : BaseController
    {
        private readonly ILogger<GroupNotificationUserController> _logger;
        private readonly IGroupNotificationUserService _groupNotificationUserService;

        private readonly IPushNotiService _pushNotiService;

        public GroupNotificationUserController(
            ILogger<GroupNotificationUserController> logger,
            IGroupNotificationUserService groupNotificationUserService,
            IPushNotiService pushNotiService
            )
        {
            _logger = logger;
            _groupNotificationUserService = groupNotificationUserService;
            _pushNotiService = pushNotiService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateGroupNotificationUserDto request)
        {
            try
            {
                var result = await _groupNotificationUserService.Create(request);

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

        [HttpPost("insert-list")]
        public async Task<IActionResult> CreateMany([FromBody] CreateManyGroupNotificationUserDto request)
        {
            try
            {
                var result = await _groupNotificationUserService.CreateMany(request);

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

        [HttpGet()]
        public async Task<IActionResult> GetUserInGroup([FromQuery] GetListUserInGroupDto request)
        {
            try
            {
                var result = await _groupNotificationUserService.GetUserInGroup(request);

                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("notification-pushing")]
        public async Task<ActionResult<ResponseContext>> PushNotificationToGroup([FromBody] PushGroupNotificationDto request)
        {
            try
            {
                await _pushNotiService.PushNotificationToGroup(request.GroupNotificationId, request.Title, request.MessageContent);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpDelete()]
        public async Task<ActionResult<ResponseContext>> RemoveUserFromGroup([FromBody] RemoveUserFromGroupNotificationDto request)
        {
            try
            {
                var result = await _groupNotificationUserService.RemoveUserFromGroup(request.UserId, request.GroupNotificationId);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg, result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseContext>> Delete(string id)
        {
            try
            {
                var result = await _groupNotificationUserService.Delete(id);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg, result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
    }
}
