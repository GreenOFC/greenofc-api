using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos.F88;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services.F88;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/lead-f88s")]
    public class LeadF88Controller : BaseController
    {
        private readonly ILogger<LeadF88Controller> _logger;
        private readonly ILeadF88Service _leadF88Service;
        private readonly IDataF88ProcessingService _dataF88ProcessingService;

        public LeadF88Controller(
             ILogger<LeadF88Controller> logger,
             ILeadF88Service leadF88Service,
             IDataF88ProcessingService dataF88ProcessingService)
        {
            _logger = logger;
            _leadF88Service = leadF88Service;
            _dataF88ProcessingService = dataF88ProcessingService;
        }

        [AllowAnonymous]
        [HttpPost("notification")]
        public async Task<IActionResult> Notification([FromBody] F88PostBackDto dto)
        {
            try
            {
                await _leadF88Service.CreateNotification(dto);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
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

        [CheckUserApprovedAuthotization]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateLeadF88Request createLeadF88Request)
        {
            try
            {
                await _leadF88Service.CreateAsync(createLeadF88Request);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetLeadF88Request getLeadF88Request)
        {
            try
            {
                var leadF88s = await _leadF88Service.GetAsync(getLeadF88Request);
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = leadF88s.Data,
                    totalrecord = leadF88s.TotalRecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("noti")]
        public async Task<IActionResult> GetNotiAsync([FromQuery] GetF88NotiRequest getF88NotiRequest)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                if (user.RoleName != "Report")
                {
                    return BadRequest(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.UNAUTHORIZED
                    });
                }
                var leadF88s = await _leadF88Service.GetNotiAsync(getF88NotiRequest);
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = leadF88s.Data,
                    totalrecord = leadF88s.TotalRecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var leadF88 = await _leadF88Service.GetDetailAsync(id);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = leadF88
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateLeadF88Request updateLeadF88Request)
        {
            try
            {
                await _leadF88Service.UpdateAsync(id, updateLeadF88Request);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("process")]
        public async Task<IActionResult> SyncDataAsync()
        {
            try
            {
                await _dataF88ProcessingService.SyncDataAsync();
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
                return BadRequest(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
