using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Services.MAFC;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.MAFC
{
    [Route("api/query/mafc")]
    public class MAFCQueryController : BaseController
    {
        private readonly ILogger<MAFCQueryController> _logger;
        private readonly IMapper _mapper;
        private readonly MAFCStatusService _statusService;
        private readonly MAFCDeferService _deferService;

        public MAFCQueryController(
            ILogger<MAFCQueryController> logger,
            IMapper mapper,
            MAFCStatusService statusService,
            MAFCDeferService deferService)
        {
            _logger = logger;
            _mapper = mapper;
            _statusService = statusService;
            _deferService = deferService;
        }

        [HttpGet("defer")]
        public ActionResult<IEnumerable<MAFCDeferModel>> GetListDefer([FromQuery] string textSearch, [FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] int? pagenumber, [FromQuery] int? pagesize)
        {
            try
            {
                var result = new List<MAFCDeferModel>();
                int totalPage = 0;
                int totalrecord = 0;
                result = _deferService.GetListByQuery(textSearch, fromDate, toDate, pagenumber, pagesize, ref totalPage, ref totalrecord);
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
                    data = result,
                    pagenumber = pagenumber.HasValue ? (int)pagenumber : 1,
                    totalpage = totalPage,
                    totalrecord = totalrecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = ex.Message,
                    data = null
                });
            }
        }


        [HttpGet("status")]
        public ActionResult<IEnumerable<MAFCStatusModel>> GetListStatus([FromQuery] string textSearch, [FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] int? pagenumber, [FromQuery] int? pagesize)
        {
            try
            {
                var result = new List<MAFCStatusModel>();
                int totalPage = 0;
                int totalrecord = 0;
                result = _statusService.GetListByQuery(textSearch, fromDate, toDate, pagenumber, pagesize, ref totalPage, ref totalrecord);
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
                    data = result,
                    pagenumber = pagenumber.HasValue ? (int)pagenumber : 1,
                    totalpage = totalPage,
                    totalrecord = totalrecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = ex.Message,
                    data = null
                });
            }
        }

        [HttpGet("account-cib")]
        public ActionResult<IEnumerable<MAFCAccountCib>> GetListAccountCib()
        {
            try
            {
                var result = new List<MAFCAccountCib>();
                result.Add(new MAFCAccountCib()
                {
                    UserName = "h79809001na",
                    Password = "1367NQMI"
                });
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
                    data = result
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
