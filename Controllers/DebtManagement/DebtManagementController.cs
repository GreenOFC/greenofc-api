using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.DebtManagement;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.DebtManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace _24hplusdotnetcore.Controllers.DebtManagement
{
    [Route("api/debt-management")]
    public class DebtManagementController : BaseController
    {
        private readonly ILogger<DebtManagementController> _logger;
        private readonly IDebtManagementService _debtManageService;
        private readonly IImportFileService _importFileService;

        public DebtManagementController(ILogger<DebtManagementController> logger, IDebtManagementService debtManageService, IImportFileService importFileService)
        {
            _logger = logger;
            _debtManageService = debtManageService;
            _importFileService = importFileService;
        }

        [HttpPost()]
        public async Task<ActionResult> CreateAsync([FromBody] CreateDebtDto request)
        {
            try
            {
                var result = await _debtManageService.Create(request);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result.Data));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _debtManageService.GetDetail(id);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result.Data));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _debtManageService.Delete(id);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result.Data));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetDebtDto request)
        {
            try
            {
                var result = await _debtManageService.GetList(request, DebtManagementImportType.COMMING.ToString());

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result.Data));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("overduedate")]
        public async Task<IActionResult> GetOverDueDateList([FromQuery] GetDebtDto request)
        {
            try
            {
                var result = await _debtManageService.GetList(request, DebtManagementImportType.OVERDUE.ToString());

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result.Data));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("import-history")]
        public async Task<IActionResult> GetImportHistory([FromQuery] ImportFilePagingRequest request)
        {
            try
            {
                var result = await _importFileService.GetList(request);

                if (result.IsSuccess)
                {
                    return Ok(ResponseContext.GetSuccessInstance(result.Data));
                }

                return BadRequest(ResponseContext.GetErrorInstance(result.ErrorMsg));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.DebtManagementCreate)]
        [HttpPost("import-overduedate")]
        public async Task<IActionResult> ImportOverDueDate(IFormFile file)
        {
            try
            {
                var result = await _debtManageService.ImportOverDueDate(file);
                return Ok(ResponseContext.GetSuccessInstance(result.Data));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.DebtManagementCreate)]
        [HttpPost("import")]
        public async Task<IActionResult> ImportExcelFile(IFormFile file)
        {
            try
            {
                var result = await _debtManageService.ImportExcel(file);
                return Ok(ResponseContext.GetSuccessInstance(result.Data));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("export")]

        public async Task<IActionResult> ExportFile([FromQuery] GetDebtDto request)
        {
            try
            {
                var result = await _debtManageService.ExportExcelFile(request);
                return File(result.Data.FileContents, result.Data.ContentType, result.Data.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("export-overduedate")]

        public async Task<IActionResult> ExportOverDueDateFile([FromQuery] GetDebtDto request)
        {
            try
            {
                var result = await _debtManageService.ExportOverDueDateExcelFile(request);
                return File(result.Data.FileContents, result.Data.ContentType, result.Data.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
