using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/permissions")]
    public class PermissionsController: BaseController
    {
        private ILogger<PermissionsController> _logger;
        private readonly IPermissionService _permissionService;

        public PermissionsController(
            IPermissionService permissionService,
            ILogger<PermissionsController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var permissionDtos = await _permissionService.GetAsync();
                return Ok(ResponseContext.GetSuccessInstance(permissionDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
