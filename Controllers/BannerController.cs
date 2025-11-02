using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.Banners;
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
    [Route("api/banners")]
    public class BannerController: BaseController
    {
        private readonly ILogger<BannerController> _logger;
        private readonly IBannerService _bannerService;

        public BannerController(
            ILogger<BannerController> logger, 
            IBannerService bannerService)
        {
            _logger = logger;
            _bannerService = bannerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetBannerRequest getBannerRequest)
        {
            try
            {
                var banners = await _bannerService.GetAsync(getBannerRequest);
                return Ok(ResponseContext.GetSuccessInstance(banners));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.BannerCreate)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateBannerRequest createBannerRequest)
        {
            try
            {
                await _bannerService.CreateAsync(createBannerRequest);
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

        [Authorize(Roles = PermissionCost.BannerUpdate)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateBannerRequest updateBannerRequest)
        {
            try
            {
                await _bannerService.UpdateAsync(id, updateBannerRequest);
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
                var banner = await _bannerService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(banner));
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

        [Authorize(Roles = PermissionCost.BannerDelete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _bannerService.DeleteAsync(id);
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
