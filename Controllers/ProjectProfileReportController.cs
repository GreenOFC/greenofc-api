using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.File;
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
    [Route("api/project-profile-reports")]
    public class ProjectProfileReportController : BaseController
    {
        private readonly ILogger<ProjectProfileReportController> _logger;
        private readonly IProjectProfileReportService _projectProfileReportService;

        public ProjectProfileReportController(
            ILogger<ProjectProfileReportController> logger,
            IProjectProfileReportService projectProfileReportService)
        {
            _logger = logger;
            _projectProfileReportService = projectProfileReportService;
        }

        [HttpPost("import")]
        [Authorize(Roles = PermissionCost.ProjectProfileReportImport)]
        public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
        {
            try
            {
                var result = await _projectProfileReportService.ImportAsync(file);
                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result));
                }
                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMessage, result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var result = await _projectProfileReportService.GetAsync(pagingRequest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] string id)
        {
            try
            {
                var result = await _projectProfileReportService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}/exports")]
        public async Task<IActionResult> ExportAsync([FromRoute] string id)
        {
            try
            {
                FileResponse file = await _projectProfileReportService.ExportAsync(id);
                return File(file.FileContents, file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
