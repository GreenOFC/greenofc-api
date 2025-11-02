using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/user")]

    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServices _userServices;
        private readonly IUserLoginService _userLoginService;

        public UserController(
            UserServices userService,
            IUserServices userServices,
            ILogger<UserController> logger,
            IUserLoginService userLoginService)
        {
            _userServices = userServices;
            _logger = logger;
            _userLoginService = userLoginService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetUserRequest getUserRequest)
        {
            try
            {
                var users = await _userServices.GetAsync(getUserRequest);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.DownloadAllUserProfile)]
        [HttpGet("download")]
        public async Task<IActionResult> DownloadAsync([FromQuery] GetUserRequest getUserRequest)
        {
            try
            {
                var users = await _userServices.GetAsync(getUserRequest);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserRequest createUserRequest)
        {
            try
            {
                await _userServices.CreateAsync(createUserRequest);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
                });
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateUserRequest updateUserRequest)
        {
            try
            {
                await _userServices.UpdateAsync(id, updateUserRequest);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
                });
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
                var user = await _userServices.GetDetailAsync(id);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = user
                });
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

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePasswordAsync(string id, ChangePasswordUserRequest changePasswordUserRequest)
        {
            try
            {
                await _userServices.ChangePasswordAsync(id, changePasswordUserRequest);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
                });
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

        [Authorize(Roles = PermissionCost.UpdateUserStatus)]
        [HttpPut("{id}/change-status")]
        public async Task<IActionResult> ChangeStatusAsync(string id, ChangeUserStatusRequest changeUserStatusRequest)
        {
            try
            {
                await _userServices.ChangeStatusAsync(id, changeUserStatusRequest);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
                });
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

        [Authorize(Roles = PermissionCost.ResetUesrPassword)]
        [HttpPut("{id}/reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(string id)
        {
            try
            {
                await _userServices.ResetPasswordAsync(id);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
                });
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

        [AllowAnonymous]
        [HttpPost("~/api/user-registration")]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest registerUserRequest)
        {
            try
            {
                var user = await _userServices.RegisterAsync(registerUserRequest);
                return Ok(ResponseContext.GetSuccessInstance(user));
                // return Ok(ResponseContext.GetErrorInstance("Tính năng tạm khóa"));
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


        // [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("create-many")]
        public async Task<ActionResult<IEnumerable<CreateListUserRequest>>> CreateManyAsync(List<CreateListUserRequest> dto)
        {
            try
            {
                if (dto == null || dto.Count == 0)
                {
                    return BadRequest(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = "Wrong Input"
                    });
                }
                var results = new List<CreateListUserRequest>();
                var teamleads = dto.Where(x => x.RoleName == "TL" || x.RoleName == "SUP").ToList();
                results = await _userServices.CreateListTeamlead(teamleads);
                var sale = dto.Where(x => x.RoleName != "TL" && x.RoleName != "SUP").ToList();
                var resultSale = await _userServices.CreateListSale(sale);
                results.AddRange(resultSale);
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpPut("update-many")]
        public async Task<ActionResult<IEnumerable<UpdateListUserDto>>> UpdateManyAsync(List<UpdateListUserDto> dto)
        {
            try
            {
                if (dto == null || dto.Count == 0)
                {
                    return BadRequest(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = "Wrong Input"
                    });
                }
                var results = await _userServices.UpdateListUser(dto);
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
        [HttpPost("delete-many")]
        public async Task<ActionResult<IEnumerable<string>>> DeleteManyAsync(IEnumerable<string> users, bool isActive)
        {
            try
            {
                if (users == null || users.Count() == 0)
                {
                    return BadRequest(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.USER_NOT_FOUND
                    });
                }
                var results = await _userServices.DeactiveManyUser(users, isActive);
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpPost("{id}/suspensions")]
        public async Task<IActionResult> CreateSuspensionHistory(string id, CreateUserSuspensionHistory createUserSuspensionHistory)
        {
            try
            {
                await _userServices.CreateSuspensionHistory(id, createUserSuspensionHistory);
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

        [HttpGet("me")]
        public async Task<IActionResult> GetDetailAsync()
        {
            try
            {
                var user = await _userServices.GetDetailAsync(_userLoginService.GetUserId());
                return Ok(ResponseContext.GetSuccessInstance(user));
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

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMeAsync(UpdateCurrentUserRequest updateCurrentUserRequest)
        {
            try
            {
                await _userServices.UpdateMeAsync(_userLoginService.GetUserId(), updateCurrentUserRequest);
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
        [HttpPut("me/document")]
        public async Task<IActionResult> UpdateDocumentAsync(UpdateDocCurrentUserRequest updateCurrentUserRequest)
        {
            try
            {
                await _userServices.UpdateDocumentsAsync(_userLoginService.GetUserId(), updateCurrentUserRequest);
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

        [Authorize(Roles = PermissionCost.ReviewUserProfile)]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveAsync(string id)
        {
            try
            {
                await _userServices.ApproveAsync(id);
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

        [Authorize(Roles = PermissionCost.ReviewUserProfile)]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectAsync(string id, RejectUserRequest rejectUserRequest)
        {
            try
            {
                await _userServices.RejectAsync(id, rejectUserRequest.Reason);
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

        [HttpDelete("remove-account")]
        public async Task<IActionResult> DeleteMeAsync([FromBody] RemoveAccountRequest removeAccountRequest)
        {
            try
            {
                await _userServices.DeleteMeAsync(removeAccountRequest.UserPassword);
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