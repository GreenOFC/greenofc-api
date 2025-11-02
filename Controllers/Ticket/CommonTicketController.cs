
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Services.Ticket;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Controllers.Ticket
{
    [AllowAnonymous]
    [Route("api/domain/comment-ticket")]
    public class CommonTicketController : BaseController
    {
        private readonly ILogger<CommonTicketController> _logger;
        private readonly IMapper _mapper;
        private readonly ITicketService _ticketService;
        private readonly ICommentTicketService _commentTicketService;

        public CommonTicketController(
            ILogger<CommonTicketController> logger,
            ITicketService ticketService,
            ICommentTicketService commentTicketService,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _ticketService = ticketService;
            _commentTicketService = commentTicketService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseContext>> CreateAsync(CreateCommentTicketModelDto dto)
        {
            try
            {
                var isExisted = await _ticketService.CheckExistedTicket(dto.TicketId);
                if (isExisted == false)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = string.Format(Common.Message.COMMON_NOT_FOUND, "Ticket"),
                        data = null
                    });
                }
                await _commentTicketService.CreateAsync(dto);
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
        [Route("{id}")]
        public async Task<ActionResult<ResponseContext>> UpdateAsync(UpdateCommentTicketModelDto dto)
        {
            try
            {
                await _commentTicketService.UpdateAsync(dto.Id, dto);
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseContext>> DeleteAsync(string id)
        {
            try
            {
                await _commentTicketService.DeleteAsync(id);
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
    }
}