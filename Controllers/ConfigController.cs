using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos.Config;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/config")]
    public class ConfigController : BaseController
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly MCConfig _mCConfig;
        private readonly PayMeConfig _paymeConfig;
        private readonly ConfigServices _configService;
        private readonly IBannerService _bannerService;
        public ConfigController(ILogger<ConfigController> logger,
        IBannerService bannerService,
        IOptions<MCConfig> mCConfigOptions,
        IOptions<PayMeConfig> paymeConfigOptions,

        ConfigServices configService)
        {
            _logger = logger;
            _configService = configService;
            _bannerService = bannerService;
            _mCConfig = mCConfigOptions.Value;
            _paymeConfig = paymeConfigOptions.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("banner")]
        public ActionResult<ResponseContext> GetBanner()
        {
            try
            {

                var banner = _bannerService.GetAllUrl();
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = banner
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("env")]
        public ActionResult<string> CheckEnv()
        {
            try
            {

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = _mCConfig.Host
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("payme-sdk")]
        public ActionResult GetPaymeSdkConfig()
        {
            try
            {
                var response = new PaymeConfigResponse()
                {
                    AppToken = _paymeConfig.AppToken,
                    SecretKey = _paymeConfig.SecretKey,
                    AppId = _paymeConfig.AppId,
                    SdkEnv = _paymeConfig.SdkEnv,
                    PublicKey = PayMeCert.PayMeSDKPubKey(_paymeConfig.SdkEnv),
                    PrivateKey = PayMeCert.PayMeSDKPriKey(_paymeConfig.SdkEnv),
                };

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }
    }
}