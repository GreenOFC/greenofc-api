using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.OCR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/ocrs")]
    public class OCRController : BaseController
    {
        private readonly ILogger<OCRController> _logger;
        private readonly IOCRService _oCRService;

        public OCRController(
            ILogger<OCRController> logger,
            IOCRService oCRService)
        {
            _logger = logger;
            _oCRService = oCRService;
        }

        [HttpPost("tranfer")]
        public async Task<IActionResult> TranferAsync([FromForm] OCRTranferRequest oCRTranferRequest)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                OCRTranferResponse oCRTranferResponse = await _oCRService.TransferAsync(oCRTranferRequest, user?.Id);

                if (oCRTranferResponse.Success)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.SUCCESS,
                        message = Common.Message.SUCCESS,
                        data = oCRTranferResponse
                    });
                }

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = oCRTranferResponse.Message,
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
        public async Task<IActionResult> GetListAsync([FromQuery]PagingRequest pagingRequest)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                PagingResponse<OCRInfoResponse> response = await _oCRService.GetListAsync(user?.Id, pagingRequest);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = response
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                OCRInfoResponse response = await _oCRService.GetAsync(id);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = response
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
