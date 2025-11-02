using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/profile")]
    public class ProfileController: BaseController
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileService _profileService;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserServices _userServices;

        public ProfileController(
            ILogger<ProfileController> logger,
            IProfileService profileService,
            IUserServices userServices,
            IUserLoginService userLoginService)
        {
            _profileService = profileService;
            _logger = logger;
            _userLoginService = userLoginService;
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailAsync()
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var user = await _profileService.GetProfileAsync(userId);
                user.Permissions = await _userServices.GetPermissionByUserId(userId);
                return Ok(ResponseContext.GetSuccessInstance(user));
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
        public async Task<IActionResult> ChangePasswordAsync(string id, ChangePasswordProfileRequest changePasswordProfileRequest)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                if(id != userId)
                {
                    return new ForbidResult();
                }

                await _profileService.ChangePasswordAsync(id, changePasswordProfileRequest);
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
    }
}
