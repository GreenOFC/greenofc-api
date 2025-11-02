using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.SaleChanelConfigUsers;
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
    [Route("api/sale-channel-config-users")]
    public class SaleChanelConfigUserController : BaseController
    {
        private readonly ILogger<SaleChanelConfigUserController> _logger;
        private readonly ISaleChanelConfigUserService _saleChanelConfigUserService;

        public SaleChanelConfigUserController(
            ILogger<SaleChanelConfigUserController> logger,
            ISaleChanelConfigUserService saleChanelConfigUserService)
        {
            _logger = logger;
            _saleChanelConfigUserService = saleChanelConfigUserService;
        }

        [HttpGet]
        [Authorize(Roles = PermissionCost.SaleChanelConfigUserGet)]
        public async Task<IActionResult> GetAsync([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var saleChanels = await _saleChanelConfigUserService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(saleChanels));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = PermissionCost.SaleChanelConfigUserCreate)]
        public async Task<IActionResult> CreateAsync(SaleChanelConfigUserCreateRequest saleChanelConfigUserCreateRequest)
        {
            try
            {
                var response = await _saleChanelConfigUserService.CreateAsync(saleChanelConfigUserCreateRequest);
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

        [HttpPut("{id}")]
        [Authorize(Roles = PermissionCost.SaleChanelConfigUserUpdate)]
        public async Task<IActionResult> UpdateAsync(string id, SaleChanelConfigUserUpdateRequest saleChanelConfigUserUpdateRequest)
        {
            try
            {
                await _saleChanelConfigUserService.UpdateAsync(id, saleChanelConfigUserUpdateRequest);
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

        [HttpGet("{id}")]
        [Authorize(Roles = PermissionCost.SaleChanelConfigUserGet)]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var saleChanelConfigUser = await _saleChanelConfigUserService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(saleChanelConfigUser));
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

        [HttpDelete("{id}")]
        [Authorize(Roles = PermissionCost.SaleChanelConfigUserDelete)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _saleChanelConfigUserService.DeleteAsync(id);
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
