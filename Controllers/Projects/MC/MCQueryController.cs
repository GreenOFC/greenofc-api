using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckInitContractModels;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.MC
{
    [Route("api/query/mc")]
    public class MCQueryController : BaseController
    {
        private readonly ILogger<MCQueryController> _logger;
        private readonly MCService _mcService;
        private readonly DataMCPrecheckService _dataMCPrecheckService;
        private readonly CustomerServices _customerService;
        private readonly CustomerDomainServices _customerDomainService;
        private readonly MCNotificationService _mcNotificationService;
        private readonly MCCheckCICService _mcCheckCICService;
        private readonly UserServices _userServices;
        private readonly NotificationServices _notificationServices;
        private readonly IMapper _mapper;
        private readonly MCConfig _mCConfig;
        private readonly IMCKiosService _mCKiosService;

        public MCQueryController(ILogger<MCQueryController> logger,
        MCService mcService,
        CustomerServices customerService,
        CustomerDomainServices customerDomainService,
        MCCheckCICService mcCheckCICService,
        MCNotificationService mcNotificationService,
        UserServices userServices,
        NotificationServices notificationServices,
        IMapper mapper,
        DataMCPrecheckService dataMCPrecheckService,
        IOptions<MCConfig> mCConfigOption,
        IMCKiosService mCKiosService)
        {
            _logger = logger;
            _mcService = mcService;
            _mcCheckCICService = mcCheckCICService;
            _mcNotificationService = mcNotificationService;
            _customerService = customerService;
            _customerDomainService = customerDomainService;
            _userServices = userServices;
            _notificationServices = notificationServices;
            _mapper = mapper;
            _dataMCPrecheckService = dataMCPrecheckService;
            _mCConfig = mCConfigOption.Value;
            _mCKiosService = mCKiosService;
        }

        [HttpGet]
        [Route("product")]
        public async Task<ActionResult<ResponseContext>> GetMCProductAsync()
        {
            try
            {
                IEnumerable<MCProduct> mCProducts = await _mcService.GetProductAsync();
                return Ok(new ResponseContext
                {
                    code = (int)ResponseCode.SUCCESS,
                    message = Message.SUCCESS,
                    data = mCProducts
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("check-list")]
        public async Task<ActionResult<ResponseContext>> CheckListAsync([FromQuery] string customerId)
        {
            try
            {
                CustomerCheckListResponseModel result = await _mcService.CheckListAsync(customerId);

                if (result?.CheckList?.Any() != true)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.NOT_FOUND_PRODUCT,
                        data = result
                    });
                }

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
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
                });
            }
        }

        [HttpGet]
        [Route("kios")]
        public async Task<ActionResult<ResponseContext>> GetKiosAsync()
        {
            try
            {
                IEnumerable<KiosModel> kios = await _mCKiosService.GetAsync();

                if (kios?.Any() != true)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.NOT_FOUND_KIOS,
                        data = kios
                    });
                }

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = kios
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

        [HttpGet("return-check-list")]
        public async Task<ActionResult<ResponseContext>> GetReturnCheckListAsync(string customerId)
        {
            try
            {
                CustomerCheckListResponseModel customerCheckListResponseModel = await _mcService.GetReturnCheckListAsync(customerId);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = customerCheckListResponseModel
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

        [HttpGet("cases")]
        public async Task<ActionResult<ResponseContext>> GetCasesAsync([FromQuery] GetCaseRequestDto getCaseRequestDto)
        {
            try
            {
                IEnumerable<GetCaseMCResponseDto> cases = await _mcService.GetCasesAsync(getCaseRequestDto);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = cases
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

        [HttpGet("list-precheck")]
        public ActionResult<IEnumerable<DataMCPrecheckModel>> ListPrecheckes([FromQuery] string textSearch, [FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] int? pagenumber, [FromQuery] int? pagesize)
        {
            try
            {
                var result = new List<DataMCPrecheckModel>();
                int totalPage = 0;
                int totalrecord = 0;
                result = _dataMCPrecheckService.GetListByQuery(textSearch, fromDate, toDate, pagenumber, pagesize, ref totalPage, ref totalrecord);
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
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                    data = null
                });
            }
        }

        [HttpGet("list-noti")]
        public ActionResult<IEnumerable<GetMCNotiResponse>> GetListNoti([FromQuery] GetMCNotiRequest request)
        {
            try
            {
                var result = _mcNotificationService.GetListNoti(request);
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
    }
}