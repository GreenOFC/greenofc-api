using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.News;
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
    [Route("api/news")]
    public class NewsController: BaseController
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsServices _newsServices;
        private readonly INotificationServices _notificationServices;

        public NewsController(
            ILogger<NewsController> logger,
            INewsServices newsServices,
            INotificationServices notificationServices)
        {
            _logger = logger;
            _newsServices = newsServices;
            _notificationServices = notificationServices;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetNewsRequest getNewsRequest)
        {
            try
            {
                var listOfNews = await _newsServices.GetAsync(getNewsRequest);
                return Ok(ResponseContext.GetSuccessInstance(listOfNews));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("top")]
        public async Task<IActionResult> GetTopAsync([FromQuery] int number)
        {
            try
            {
                var listOfNews = await _newsServices.GetTopAsync(number);
                return Ok(ResponseContext.GetSuccessInstance(listOfNews));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.NewsCreate)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateNewsRequest createNewsRequest)
        {
            try
            {
                await _newsServices.CreateAsync(createNewsRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.NewsUpdate)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateNewsRequest updateNewsRequest)
        {
            try
            {
                await _newsServices.UpdateAsync(id, updateNewsRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var news = await _newsServices.GetDetailAsync(id);
                await _notificationServices.ReadNewsAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(news));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.NewsDelete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _newsServices.DeleteAsync(id);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
