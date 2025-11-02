using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.File;
using _24hplusdotnetcore.ModelDtos.StorageModels;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.Files;
using _24hplusdotnetcore.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : BaseController
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IFileService _fileService;
        private readonly CustomerQueryService _customerQueryService;
        private readonly IStorageService _storageService;

        public FilesController(
            ILogger<FilesController> logger,
            CustomerQueryService customerQueryService,
            IFileService fileService,
            IStorageService storageService)
        {
            _logger = logger;
            _customerQueryService = customerQueryService;
            _fileService = fileService;
            _storageService = storageService;
        }

        /// <summary>
        /// Download file with type
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DownloadAsync([FromQuery] FileRequestDto fileRequest)
        {
            try
            {
                FileResponse file = await _fileService.DownloadAsync(fileRequest);
                return File(file.FileContents, file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Test Upload files
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadAsync(string customerId, [FromForm]IEnumerable<IFormFile> files)
        {
            try
            {
                var response = new List<StorageFileResponse>();
                foreach (var file in files)
                {
                    byte[] bytes = await file.GetBytes();
                    var result = await _storageService.UploadFileAsync(customerId, file.FileName, bytes);
                    response.Add(result);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
