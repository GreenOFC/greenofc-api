using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.SaleChanels;
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
    [Route("api/sale-channels")]
    public class SaleChanelController : BaseController
    {
        private readonly ILogger<SaleChanelController> _logger;
        private readonly ISaleChanelService _saleChanelService;

        public SaleChanelController(
            ILogger<SaleChanelController> logger,
            ISaleChanelService saleChanelService)
        {
            _logger = logger;
            _saleChanelService = saleChanelService;
        }

        [HttpGet]
        [Authorize(Roles = PermissionCost.SaleChanelGet)]
        public async Task<IActionResult> GetAsync([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var saleChanels = await _saleChanelService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(saleChanels));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = PermissionCost.SaleChanelCreate)]
        public async Task<IActionResult> CreateAsync(SaleChanelCreateRequest saleChanelCreateRequest)
        {
            try
            {
                var response = await _saleChanelService.CreateAsync(saleChanelCreateRequest);
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
        [Authorize(Roles = PermissionCost.SaleChanelCreate)]
        public async Task<IActionResult> UpdateAsync(string id, SaleChanelUpdateRequest saleChanelUpdateRequest)
        {
            try
            {
                await _saleChanelService.UpdateAsync(id, saleChanelUpdateRequest);
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
        [Authorize(Roles = PermissionCost.SaleChanelGet)]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var saleChanel = await _saleChanelService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(saleChanel));
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
        [Authorize(Roles = PermissionCost.SaleChanelDelete)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _saleChanelService.DeleteAsync(id);
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

        [HttpPost("{id}/poses")]
        public async Task<IActionResult> AddPosAsync(string id, SaleChanelPosRequest saleChanelPosRequest)
        {
            try
            {
                await _saleChanelService.AddPosAsync(id, saleChanelPosRequest.PosId);
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

        [HttpDelete("{id}/poses")]
        public async Task<IActionResult> DeletePosAsync(string id, SaleChanelPosRequest saleChanelPosRequest)
        {
            try
            {
                await _saleChanelService.DeletePosAsync(id, saleChanelPosRequest.PosId);
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

        [HttpGet("{id}/poses")]
        public async Task<IActionResult> GetPosAsync(string id, [FromQuery] SaleChanelPosGetListRequest pagingRequest)
        {
            try
            {
                var poses = await _saleChanelService.GetPosAsync(id, pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(poses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("sync-user")]
        [Authorize(Roles = PermissionCost.SaleChanelSync)]
        public async Task<IActionResult> SyncUserAsync()
        {
            try
            {
                await _saleChanelService.SyncUserAsync();
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
