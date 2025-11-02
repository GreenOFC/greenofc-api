
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
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
    [Route("api/query/ticket")]
    public class QueryTicketController : BaseController
    {
        private readonly ILogger<QueryTicketController> _logger;
        private readonly IMapper _mapper;
        private readonly ITicketService _ticketService;

        public QueryTicketController(
            ILogger<QueryTicketController> logger,
            ITicketService ticketService,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _ticketService = ticketService;
        }
        [HttpGet("get-list")]
        public async Task<ActionResult<IEnumerable<GetTicketResponse>>> GetListAsync([FromQuery] GetTicketRequest request)
        {
            try
            {
                var result = await _ticketService.GetListAsync(request);
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
                    data = result.Data,
                    totalrecord = result.TotalRecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                    data = null
                });
            }
        }

        [HttpGet("report")]
        [Authorize(Roles = PermissionCost.TicketReport)]
        public async Task<ActionResult<IEnumerable<GetTicketResponse>>> GetReportAsync([FromQuery] GetReportTicketRequest request)
        {
            try
            {
                var result = await _ticketService.GetReportAsync(request);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(ResponseContext.GetErrorInstance(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketResponse>> GetOneAsync(string id)
        {
            try
            {
                var result = await _ticketService.GetDetailAsync(id);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                    data = null
                });
            }
        }
    }
}