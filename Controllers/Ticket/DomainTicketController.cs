
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.ModelDtos.StorageModels;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Services.Ticket;
using _24hplusdotnetcore.Services.Storage;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace _24hplusdotnetcore.Controllers.Ticket
{
    [AllowAnonymous]
    [Route("api/domain/ticket")]
    public class DomainTicketController : BaseController
    {
        private readonly ILogger<DomainTicketController> _logger;
        private readonly IMapper _mapper;
        private readonly ITicketService _ticketService;
        private readonly IStorageService _storageService;

        public DomainTicketController(
            ILogger<DomainTicketController> logger,
            ITicketService ticketService,
            IMapper mapper,
            IStorageService storageService)
        {
            _logger = logger;
            _mapper = mapper;
            _ticketService = ticketService;
            _storageService = storageService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseContext>> CreateAsync(CreateTicketModelDto dto)
        {
            try
            {
                await _ticketService.CreateAsync(dto);
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
                    message = ex.Message,
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseContext>> UpdateAsync(UpdateTicketModelDto dto)
        {
            try
            {
                await _ticketService.UpdateAsync(dto.Id, dto);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = null
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseContext>> DeleteAsync(string id)
        {
            try
            {
                await _ticketService.DeleteAsync(id);
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
                    message = ex.Message,
                });
            }
        }

        [HttpPut]
        [Route("{id}/change-status")]
        public async Task<ActionResult<ResponseContext>> ChangeStatus(string id, string status)
        {
            try
            {
                await _ticketService.ChangeStatus(id, status);
                var result = await _ticketService.GetDetailAsync(id);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }
        
        /// <summary>
        /// Test Upload files for ticket and comment ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadImageAsync(string ticketId, [FromForm] IEnumerable<IFormFile> files)
        {
            try
            {
                var response = new List<StorageFileResponse>();
                foreach (var file in files)
                {
                    byte[] bytes = await file.GetBytes();
                    var result = await _storageService.UploadFileAsync(ticketId, file.FileName, bytes);
                    response.Add(result);
                }

                return Ok(response);
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