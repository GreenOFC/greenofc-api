using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Notification;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.Users;
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
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthServices _authServices;
        private readonly AuthRefreshServices _authRefreshServices;
        private readonly UserLoginServices _userLoginServices;
        private readonly NotificationServices _notificationServices;
        private readonly MobileVersionServices _mobileVersionServices;
        private readonly IUserServices _userServices;
        private readonly IPosService _posService;

        public AuthController(
            AuthServices authServices,
            AuthRefreshServices authRefreshServices,
            UserLoginServices userLoginServices,
            NotificationServices notificationServices,
            ILogger<AuthController> logger,
            MobileVersionServices mobileVersionServices,
            IUserServices userServices,
            IPosService posService)
        {
            _logger = logger;
            _authServices = authServices;
            _authRefreshServices = authRefreshServices;
            _userLoginServices = userLoginServices;
            _notificationServices = notificationServices;
            _mobileVersionServices = mobileVersionServices;
            _userServices = userServices;
            _posService = posService;
        }

        [Authorize]
        [HttpPost]
        [Route("login")]
        public ActionResult Login(User user)
        {
            try
            {
                var authInfo = new AuthInfo();
                authInfo = _authServices.Login(user);
                if (authInfo != null)
                {
                    var authRefresh = new AuthRefresh();
                    authRefresh.UserName = authInfo.UserName;
                    authRefresh.RefresToken = authInfo.RefreshToken;
                    var newAuthRefresh = new AuthRefresh();
                    newAuthRefresh = _authRefreshServices.CreateAuthRefresh(authRefresh);
                    if (newAuthRefresh != null)
                    {
                        return Ok(authInfo);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = Common.Message.LOGIN_BIDDEN });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = Common.Message.INCORRECT_USERNAME_PASSWORD });
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }

        }

        [Route("logout")]
        [HttpPut]
        public async Task<ActionResult> LogOutAsync()
        {
            try
            {

                await _userLoginServices.RemoveRegistrationToken();

                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("userlogin")]
        public async Task<ActionResult> LoginAsync(RequestLoginInfo requestLoginInfo)
        {
            try
            {
                bool isLastedVersion = _mobileVersionServices.IsLastedVersion(requestLoginInfo.Ostype, requestLoginInfo.MobileVersion);
                if (!isLastedVersion)
                {
                    return StatusCode(StatusCodes.Status426UpgradeRequired, new ResponseContext
                    {
                        code = (int)ResponseCode.ERROR,
                        message = Message.INCORRECT_VERSION
                    });
                }

                var resLogin = await _authServices.LoginWithoutRefeshTokenAsync(requestLoginInfo);
                if(resLogin == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseContext
                    {
                        code = (int)ResponseCode.ERROR,
                        message = Message.INCORRECT_USERNAME_PASSWORD,
                        data = null
                    });
                }

                var newsRequest = new GetNotificationRequest()
                {
                    UserId = resLogin.UserId,
                    GreenType = string.Empty,
                    IsUnread = true
                };
                // get unread notification
                resLogin.unReadNoti = await _notificationServices.CountAsync(newsRequest);
                resLogin.Permissions = await _userServices.GetPermissionByUserId(resLogin.UserId);

                return Ok(new ResponseContext
                {
                    code = (int)ResponseCode.SUCCESS,
                    message = Message.LOGIN_SUCCESS,
                    data = resLogin
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequestDto registerUserRequest)
        {
            try
            {
                var user = await _userServices.RegisterUserAsync(registerUserRequest);
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

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyAsync(UserVerifyRequest userVerifyRequest)
        {
            try
            {
                var user = await _userServices.VerifyAsync(userVerifyRequest);
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

        [AllowAnonymous]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendVerifyAsync(UserSendVerifyRequest userSendVerifyRequest)
        {
            try
            {
                await _userServices.SendVerifyAsync(userSendVerifyRequest);
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

        [AllowAnonymous]
        [HttpPost("send-reset-password")]
        public async Task<IActionResult> SendResetPasswordAsync(UserSendResetPasswordRequest userSendResetPasswordRequest)
        {
            try
            {
                var result = await _userServices.SendResetPasswordAsync(userSendResetPasswordRequest);
                return Ok(ResponseContext.GetSuccessInstance(result));
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

        /// <summary>
        /// API confirm OTP when user reset password
        /// </summary>
        /// <param name="userConfirmOtpRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("send-confirm-otp")]
        public async Task<IActionResult> SendConfirmOtpAsync(UserConfirmOtpRequest userConfirmOtpRequest)
        {
            try
            {
                var response = await _userServices.SendConfirmOtpAsync(userConfirmOtpRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
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

        [AllowAnonymous]
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPasswordAsync(UserSetPasswordRequest userSetPasswordRequest)
        {
            try
            {
                await _userServices.SetPasswordAsync(userSetPasswordRequest);
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


        [AllowAnonymous]
        [HttpGet("pos")]
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

        [AllowAnonymous]
        [HttpGet("team-leads")]
        public async Task<IActionResult> GetTeamLeadAsync([FromQuery] GetTeamLeadRequest getTeamLeadRequest)
        {
            try
            {
                getTeamLeadRequest.RoleName = RoleNameEnum.TL;
                var user = await _userServices.GetTeamLeadAsync(getTeamLeadRequest);
                return Ok(ResponseContext.GetSuccessInstance(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("asm")]
        public async Task<IActionResult> GetAsmAsync([FromQuery] GetTeamLeadRequest getTeamLeadRequest)
        {
            try
            {
                getTeamLeadRequest.RoleName = RoleNameEnum.ASM;
                var user = await _userServices.GetTeamLeadAsync(getTeamLeadRequest);
                return Ok(ResponseContext.GetSuccessInstance(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("referral")]
        public async Task<IActionResult> ReferralAsync([FromQuery] GetUserReferralRequest getUserReferralRequest)
        {
            try
            {
                var user = await _userServices.GetUserReferralAsync(getUserReferralRequest);
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
    }
}