using _24hplusdotnetcore.ModelDtos.FileUploads;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    public class FileUploadController : BaseController
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly FileUploadServices _fileUploadServices;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IStorageService _storageService;

        public FileUploadController(
            ILogger<FileUploadController> logger, 
            FileUploadServices fileUploadServices, 
            IWebHostEnvironment hostingEnvironment,
            IStorageService storageService)
        {
            _logger = logger;
            _fileUploadServices = fileUploadServices;
            _hostingEnvironment = hostingEnvironment;
            _storageService = storageService;
        }
        [HttpGet]
        [Route("api/fileuploads/{CustomerId}")]
        public ActionResult<ResponseContext> GetFileUploadsById(string CustomerId)
        {
            try
            {
                var lstFileUpload = new List<FileUpload>();
                lstFileUpload = _fileUploadServices.GetListFileUploadByCustomerId(CustomerId);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = lstFileUpload
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("api/fileupload/create")]
        public ActionResult<ResponseContext> Create(FileUpload fileUpload)
        {
            try
            {
                var newFileUpload = _fileUploadServices.CreateFileUpload(fileUpload);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = newFileUpload
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("api/fileupload/update")]
        public ActionResult<ResponseContext> Update([FromBody] FileUpload[] fileUploads)
        {
            try
            {
                var updateCount = _fileUploadServices.UpdateFileUpLoad(fileUploads);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = JsonConvert.SerializeObject("" + updateCount + " records have been updated")
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
        [HttpGet]
        [Route("api/fileupload/delete")]
        public async Task<IActionResult> FileUploadDelete([FromQuery] string customerId, [FromQuery] string fileName)
        {
            try
            {
                await _storageService.DeleteFileAsync(customerId, fileName);
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
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = Common.Message.ERROR,
                    data = null
                });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/fileupload/upload")]
        public async Task<ActionResult> UploadAsync([FromForm(Name = "")] IFormFile file, [FromForm] string DocumentCategoryId, [FromForm] string CustomerId)
        {
            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                var result = await _storageService.UploadFileAsync(CustomerId, file.FileName, fileBytes);

                var fileUpload = new FileUpload
                {
                    CustomerId = CustomerId,
                    DocumentCategoryId = DocumentCategoryId,
                    FileUploadName = file.FileName,
                    FileUploadURL = result.AbsolutePath
                };

                var newFileUpload = _fileUploadServices.CreateFileUpload(fileUpload);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = newFileUpload
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
        [HttpPost]
        [Route("api/fileupload/single-upload")]
        public async Task<ActionResult> SingleUploadAsync([FromForm(Name = "file")] IFormFile file, [FromForm] string CustomerId)
        {
            try
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var result = await _storageService.UploadFileAsync(CustomerId, file.FileName, fileBytes);

                var fileUpload = new FileUpload
                {
                    CustomerId = CustomerId,
                    DocumentCategoryId = null,
                    FileUploadName = file.FileName,
                    FileUploadURL = result.AbsolutePath
                };

                var newFileUpload = _fileUploadServices.CreateFileUpload(fileUpload);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = newFileUpload
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
        [HttpPost("api/fileupload")]
        public async Task<ActionResult> UploadAsync([FromForm] CreateFileUpdateRequest createFileUpdateV1Request)
        {
            try
            {
                var fileBytes = await createFileUpdateV1Request.GetFileByteAsync();
                var fileName = createFileUpdateV1Request.GetFileName();
                var result = await _storageService.UploadFileAsync(createFileUpdateV1Request.BasePath, fileName, fileBytes);

                var fileUpload = new FileUpload
                {
                    BasePath = createFileUpdateV1Request.BasePath,
                    DocumentCategoryId = createFileUpdateV1Request.DocumentCategoryId,
                    FileUploadName = fileName,
                    FileUploadURL = result.AbsolutePath
                };

                var newFileUpload = _fileUploadServices.CreateFileUpload(fileUpload);

                return Ok(ResponseContext.GetSuccessInstance(newFileUpload));
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