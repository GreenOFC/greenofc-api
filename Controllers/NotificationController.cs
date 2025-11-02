using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos.Notification;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/notification")]

    public class NotificationController : BaseController
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly NotificationServices _notificationServices;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBannerService _bannerService;
        private readonly INewsServices _newsServices;

        public NotificationController(
            ILogger<NotificationController> logger,
            NotificationServices notificationServices,
            ICustomerRepository customerRepository,
            IBannerService bannerService,
            INewsServices newsServices
        )
        {
            _logger = logger;
            _notificationServices = notificationServices;
            _customerRepository = customerRepository;
            _bannerService = bannerService;
            _newsServices = newsServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetNotificationRequest getNewsRequest)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                getNewsRequest.UserId = user.Id;
                var listOfNews = await _notificationServices.GetAsync(getNewsRequest);
                return Ok(ResponseContext.GetSuccessInstance(listOfNews));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("update")]
        public async Task<ActionResult<ResponseContext>> UpdateAsync([FromQuery] string id, [FromQuery] bool isRead)
        {
            try
            {
                await _notificationServices.UpdateIsReadAsync(id, isRead);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = null
                });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("home")]
        public async Task<ActionResult<ResponseContext>> GetHomeNotiAsync()
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                var result = new HomeNotification();
                var newsRequest = new GetNotificationRequest() {
                    UserId = user.Id,
                    GreenType = GreenType.News,
                    IsUnread = true
                };
                result.Unsecured = await _customerRepository.CountReturnAsync(user.UserName);
                result.News = await _notificationServices.CountAsync(newsRequest);
                result.Banner = _bannerService.GetAllUrl();
                result.TopNews = await _newsServices.GetTopAsync(5);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }
    }
}